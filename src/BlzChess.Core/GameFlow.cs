namespace BlzChess.Core;

/// <summary>What the page is showing.</summary>
public enum GameStage
{
    Playing,
    Finished,
}

/// <summary>
/// The page's state machine, kept out of the component so it can be tested: choose a
/// level, play, finish, and a new game asks for a level again.
/// </summary>
public sealed class GameFlow
{
    private readonly IEngine _engine;
    private readonly Func<int> _seed;

    public GameFlow(IEngine engine, Func<int>? seedFactory = null)
        : this(engine, Level.Weakest, seedFactory)
    {
    }

    public GameFlow(IEngine engine, Level openingLevel, Func<int>? seedFactory = null)
    {
        _engine = engine ?? throw new ArgumentNullException(nameof(engine));
        _seed = seedFactory ?? (() => Environment.TickCount);
        Start(openingLevel);
    }

    private int _suggestionToken;

    public GameStage Stage { get; private set; } = GameStage.Playing;

    /// <summary>Whether the page shows a suggested move. Off until the player asks.</summary>
    public bool Hints { get; private set; }

    /// <summary>
    /// The move the engine considers best for the player in the position on the board, or
    /// nothing while there is none to show. A suggestion is never played.
    /// </summary>
    public Move? Suggestion { get; private set; }

    public GameSession Session { get; private set; } = default!;

    public Level Level => Session.Level;

    public GameResult Result => Session.Result;

    public bool TryPlayerMove(Move move, out Move applied)
    {
        applied = default;
        if (Stage != GameStage.Playing)
        {
            return false;
        }

        if (!Session.TryPlayerMove(move, out applied))
        {
            return false;
        }

        // The suggestion was about the position before this move.
        DropSuggestion();
        UpdateStage();
        return true;
    }

    public async Task<Move?> PlayEngineMoveAsync(CancellationToken cancellationToken = default)
    {
        if (Stage != GameStage.Playing)
        {
            return null;
        }

        var move = await Session.PlayEngineMoveAsync(cancellationToken).ConfigureAwait(false);
        UpdateStage();

        // The position the suggestion described is gone with the move; ask about the new
        // one so the arrow the player sees belongs to the board they are looking at.
        await SuggestAsync(cancellationToken).ConfigureAwait(false);
        return move;
    }

    /// <summary>
    /// Turns the suggested move on or off. On asks the engine; off takes the arrow away
    /// and cancels any answer still on its way.
    /// </summary>
    public async Task ToggleHintsAsync(CancellationToken cancellationToken = default)
    {
        Hints = !Hints;
        if (!Hints)
        {
            _suggestionToken++;
            Suggestion = null;
            return;
        }

        await SuggestAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Asks the engine for the best move in the position on the board and keeps the
    /// answer if it is still wanted when it arrives. Never blocks: the caller does not
    /// wait to let the player move.
    /// </summary>
    public async Task SuggestAsync(CancellationToken cancellationToken = default)
    {
        if (!Hints || Stage != GameStage.Playing || Session.IsEngineThinking)
        {
            Suggestion = null;
            return;
        }

        var position = Session.Game.Position;
        if (position.SideToMove != Session.PlayerColor)
        {
            Suggestion = null;
            return;
        }

        var token = ++_suggestionToken;
        var fen = Session.Game.Fen;
        var legal = Session.Game.LegalMoves().ToHashSet();
        var answer = await _engine.BestMoveAsync(fen, Level.Coach, cancellationToken).ConfigureAwait(false);

        // A slow answer can arrive after the player moved, took a move back, started a new
        // game, or switched hints off. An arrow for a position that is no longer on the
        // board is worse than no arrow, so the token decides.
        if (token != _suggestionToken || !Hints || Stage != GameStage.Playing)
        {
            return;
        }

        Suggestion = answer is { } move && legal.Contains(move) ? move : null;
    }

    /// <summary>
    /// Changes the opponent for its next move, only while a game is in progress. The
    /// game itself is untouched.
    /// </summary>
    public bool ChangeLevel(Level level)
    {
        ArgumentNullException.ThrowIfNull(level);
        Session.ChangeLevel(level);
        return true;
    }

    /// <summary>
    /// Takes back the player's last move and the computer's answer, only while a game
    /// is running. A finished game stays finished.
    /// </summary>
    public bool UndoLastMove()
    {
        if (Stage != GameStage.Playing || !Session.UndoPlayerTurn())
        {
            return false;
        }

        DropSuggestion();
        return true;
    }

    /// <summary>
    /// Starts the same level again with a fresh board, and records nothing: the game
    /// that was in progress is gone, with no result and no winner. Refused unless a
    /// game is running, because there is nothing to restart otherwise.
    /// </summary>
    public bool Restart()
    {
        if (Stage != GameStage.Playing)
        {
            return false;
        }

        DropSuggestion();
        Start(Level);
        return true;
    }

    /// <summary>
    /// Starts a fresh game at the level shown. This is the way out of a finished game,
    /// because a page that always holds a game has nothing else to go back to.
    /// </summary>
    public void NewGame()
    {
        DropSuggestion();
        Start(Level);
    }

    /// <summary>
    /// Forgets the suggestion without asking for a new one. Used where the position moved
    /// without a move being made: a take-back, a restart, a new game.
    /// </summary>
    private void DropSuggestion()
    {
        _suggestionToken++;
        Suggestion = null;
    }

    private void Start(Level level)
    {
        ArgumentNullException.ThrowIfNull(level);
        Session = new GameSession(level, _engine, _seed());
        Stage = GameStage.Playing;
        DropSuggestion();
    }

    private void UpdateStage()
    {
        if (Session.Result.IsOver)
        {
            Stage = GameStage.Finished;
        }
    }
}