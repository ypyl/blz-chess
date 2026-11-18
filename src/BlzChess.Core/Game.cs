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
    private readonly List<Piece> _captured = [];
    private readonly List<string> _positionKeys = [];
    private readonly Position _initialPosition;

    public Game()
        : this(Position.Initial)
    {
    }

    public Game(Position position)
    {
        ArgumentNullException.ThrowIfNull(position);
        _initialPosition = position.Clone();
        Position = position;
        _positionKeys.Add(position.PlacementKey());
    }

    public Position Position { get; private set; }

    public IReadOnlyList<Move> Moves => _moves;

    /// <summary>
    /// The pieces taken off the board, in the order they were taken. Recorded as each move
    /// is applied, so the replay behind a take-back rebuilds it with the moves instead of
    /// a second rule that could drift from them.
    /// </summary>
    public IReadOnlyList<Piece> Captured => _captured;

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

        _moves.Add(move);
        Apply(move);
        applied = move;
        return true;
    }

    /// <summary>
    /// Applies a move the caller has already found legal and recorded, noting anything it
    /// takes. One site, so a move played now and the same move replayed after a take-back
    /// cannot disagree about what was taken.
    /// </summary>
    private void Apply(Move move)
    {
        // Read before the move, because the move is what empties the square.
        if (TakenBy(Position, move) is { } taken)
        {
            _captured.Add(taken);
        }

        Position = Position.ApplyUnchecked(move);
        _positionKeys.Add(Position.PlacementKey());
    }

    /// <summary>
    /// What <paramref name="move"/> takes, or nothing. A pawn landing diagonally on an
    /// empty square is taking in passing, and the pawn it takes is beside it, not on the
    /// square it landed on.
    /// </summary>
    private static Piece? TakenBy(Position position, Move move)
    {
        if (position[move.To] is { } onTarget)
        {
            return onTarget;
        }

        if (position[move.From] is { Type: PieceType.Pawn } pawn
            && Squares.FileOf(move.From) != Squares.FileOf(move.To))
        {
            return new Piece(pawn.Color == Color.White ? Color.Black : Color.White, PieceType.Pawn);
        }

        return null;
    }

    /// <summary>True when the move is legal in the current position.</summary>
    public bool IsLegal(Move move) => !Result.IsOver && MoveGenerator.LegalMoves(Position).Contains(move);

    /// <summary>
    /// Drops the last <paramref name="count"/> moves and rebuilds the position by
    /// replaying what is left, from the position the game started from. There is no
    /// reverse for <see cref="Position.ApplyUnchecked"/>: castling moves rooks, en
    /// passant clears squares nobody moved to, and rights and clocks shift. A replay
    /// of moves that were legal when they were played is the same position, exactly.
    /// </summary>
    public bool TrimMoves(int count)
    {
        if (count is < 1 || count > _moves.Count)
        {
            return false;
        }

        _moves.RemoveRange(_moves.Count - count, count);
        Position = _initialPosition.Clone();
        _positionKeys.Clear();
        _positionKeys.Add(Position.PlacementKey());
        _captured.Clear();
        foreach (var move in _moves)
        {
            Apply(move);
        }

        return true;
    }

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