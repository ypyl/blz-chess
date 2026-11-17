namespace BlzChess.Core;

/// <summary>
/// One game between the player, who is always White, and the computer at a chosen
/// level. It owns the turn order: the player moves, then the engine answers, except
/// when the level says to play a random legal move.
/// </summary>
public sealed class GameSession
{
    private readonly IEngine _engine;
    private readonly Random _random;

    public GameSession(Level level, IEngine engine, int seed = 0)
    {
        Level = level ?? throw new ArgumentNullException(nameof(level));
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        _random = new Random(seed);
        Game = new Game();
    }

    public Level Level { get; private set; }

    public Game Game { get; }

    public Color PlayerColor => Color.White;

    public bool IsEngineThinking { get; private set; }

    public GameResult Result => Game.Result;

    /// <summary>
    /// Changes the opponent for its next move. The position, the moves played, and
    /// the side to move are none of this method's business, which is the point: a
    /// player who has misjudged the level keeps the game.
    /// </summary>
    public void ChangeLevel(Level level)
    {
        Level = level ?? throw new ArgumentNullException(nameof(level));
    }

    public string Fen => Game.Fen;

    public List<Move> LegalMovesFrom(int square) => Game.LegalMovesFrom(square);

    /// <summary>
    /// Plays the player's move. Returns false when it is not the player's turn, when
    /// the game is over, or when the move is illegal, and in every one of those cases
    /// the position is unchanged.
    /// </summary>
    public bool TryPlayerMove(Move move, out Move applied)
    {
        applied = default;
        if (IsEngineThinking
            || Result.IsOver
            || Game.Position.SideToMove != PlayerColor)
        {
            return false;
        }

        return Game.TryMove(move, out applied);
    }

    /// <summary>
    /// The computer's reply, already played. Null when there is nothing to play
    /// because the game is over or it is not the computer's turn.
    /// </summary>
    public async Task<Move?> PlayEngineMoveAsync(CancellationToken cancellationToken = default)
    {
        if (Result.IsOver || Game.Position.SideToMove == PlayerColor)
        {
            return null;
        }

        var legal = Game.LegalMoves();
        if (legal.Count == 0)
        {
            return null;
        }

        IsEngineThinking = true;
        try
        {
            var chosen = ChooseLevelDecidesRandomly(legal)
                ? legal[_random.Next(legal.Count)]
                : await AskEngine(legal, cancellationToken).ConfigureAwait(false);

            _ = Game.TryMove(chosen, out var applied);
            return applied;
        }
        finally
        {
            IsEngineThinking = false;
        }
    }

    private bool ChooseLevelDecidesRandomly(List<Move> legal) =>
        Level.RandomChance > 0 && _random.NextDouble() < Level.RandomChance && legal.Count > 1;

    private async Task<Move> AskEngine(List<Move> legal, CancellationToken cancellationToken)
    {
        var reply = await _engine.BestMoveAsync(Fen, Level, cancellationToken).ConfigureAwait(false);
        if (reply is { } move && legal.Contains(move))
        {
            return move;
        }

        // An engine that cannot answer, or answers with something illegal, must not
        // stall the game: any legal move keeps it playable.
        return legal[_random.Next(legal.Count)];
    }
}