namespace BlzChess.Core;

/// <summary>How a game ended, or that it has not.</summary>
public enum ResultKind
{
    InProgress,
    Checkmate,
    Stalemate,
    ThreefoldRepetition,
    FiftyMove,
    InsufficientMaterial,
}

/// <summary>The outcome of a game, with the winner when there is one.</summary>
public readonly record struct GameResult(ResultKind Kind, Color? Winner)
{
    public static GameResult InProgress => new(ResultKind.InProgress, null);

    public bool IsOver => Kind != ResultKind.InProgress;

    /// <summary>A sentence for the page, in the player's voice where it matters.</summary>
    public string Describe(Color playerColor) => Kind switch
    {
        ResultKind.InProgress => "Your move.",
        ResultKind.Checkmate when Winner == playerColor => "Checkmate. You win.",
        ResultKind.Checkmate => "Checkmate. The computer wins.",
        ResultKind.Stalemate => "Stalemate. A draw.",
        ResultKind.ThreefoldRepetition => "Draw by repetition.",
        ResultKind.FiftyMove => "Draw by the fifty-move rule.",
        ResultKind.InsufficientMaterial => "Draw by insufficient material.",
        _ => "Game over.",
    };
}

/// <summary>
/// A game in progress: the position, the moves played, and the result computed
/// from them. It refuses moves that are not legal for the side to move.
/// </summary>
public sealed class Game
{
    private readonly List<Move> _moves = [];
    private readonly List<string> _positionKeys = [];

    public Game()
        : this(Position.Initial)
    {
    }

    public Game(Position position)
    {
        ArgumentNullException.ThrowIfNull(position);
        Position = position;
        _positionKeys.Add(position.PlacementKey());
    }

    public Position Position { get; private set; }

    public IReadOnlyList<Move> Moves => _moves;

    public GameResult Result => ComputeResult();

    public string Fen => Position.ToFen();

    public List<Move> LegalMoves() => MoveGenerator.LegalMoves(Position);

    /// <summary>
    /// Plays <paramref name="move"/> when it is legal. Returns false and changes
    /// nothing otherwise, so a caller needs no separate validation.
    /// </summary>
    public bool TryMove(Move move, out Move applied)
    {
        applied = default;
        if (Result.IsOver)
        {
            return false;
        }

        var legal = MoveGenerator.LegalMoves(Position);
        if (!legal.Contains(move))
        {
            return false;
        }

        Position = Position.ApplyUnchecked(move);
        _moves.Add(move);
        _positionKeys.Add(Position.PlacementKey());
        applied = move;
        return true;
    }

    /// <summary>True when the move is legal in the current position.</summary>
    public bool IsLegal(Move move) => !Result.IsOver && MoveGenerator.LegalMoves(Position).Contains(move);

    /// <summary>Moves from one square, for the page's highlighting. Empty for an illegal source.</summary>
    public List<Move> LegalMovesFrom(int from) =>
        [.. MoveGenerator.LegalMoves(Position).Where(move => move.From == from)];

    private GameResult ComputeResult()
    {
        var legal = MoveGenerator.LegalMoves(Position);
        if (legal.Count == 0)
        {
            return MoveGenerator.IsInCheck(Position)
                ? new GameResult(ResultKind.Checkmate, Position.SideToMove == Color.White ? Color.Black : Color.White)
                : new GameResult(ResultKind.Stalemate, null);
        }

        if (Position.HalfmoveClock >= 100)
        {
            return new GameResult(ResultKind.FiftyMove, null);
        }

        var key = Position.PlacementKey();
        if (_positionKeys.Count(seen => seen == key) >= 3)
        {
            return new GameResult(ResultKind.ThreefoldRepetition, null);
        }

        return IsInsufficientMaterial()
            ? new GameResult(ResultKind.InsufficientMaterial, null)
            : GameResult.InProgress;
    }

    /// <summary>
    /// King against king, or with one bishop or knight on the board. Anything with
    /// pawns or rooks can still be won, and two bishops or a bishop and knight can
    /// force mate, so those games continue.
    /// </summary>
    private bool IsInsufficientMaterial()
    {
        var minors = 0;
        for (var square = 0; square < 64; square++)
        {
            var piece = Position[square];
            if (piece is null || piece.Value.Type == PieceType.King)
            {
                continue;
            }

            switch (piece.Value.Type)
            {
                case PieceType.Bishop:
                case PieceType.Knight:
                    minors++;
                    break;
                default:
                    return false;
            }
        }

        return minors <= 1;
    }
}