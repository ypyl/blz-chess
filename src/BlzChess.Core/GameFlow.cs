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

    public GameStage Stage { get; private set; } = GameStage.Playing;

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
        return move;
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

        Start(Level);
        return true;
    }

    /// <summary>
    /// Starts a fresh game at the level shown. This is the way out of a finished game,
    /// because a page that always holds a game has nothing else to go back to.
    /// </summary>
    public void NewGame() => Start(Level);

    private void Start(Level level)
    {
        ArgumentNullException.ThrowIfNull(level);
        Session = new GameSession(level, _engine, _seed());
        Stage = GameStage.Playing;
    }

    private void UpdateStage()
    {
        if (Session.Result.IsOver)
        {
            Stage = GameStage.Finished;
        }
    }
}