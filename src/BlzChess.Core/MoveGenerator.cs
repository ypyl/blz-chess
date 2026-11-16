namespace BlzChess.Core;

/// <summary>
/// Legal move generation. Pseudo-legal moves are produced first, then each is
/// tried on a copy and thrown away when it leaves the mover's king attacked.
/// </summary>
public static class MoveGenerator
{
    private static readonly (int File, int Rank)[] KnightSteps =
    [
        (1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2),
    ];

    private static readonly (int File, int Rank)[] BishopSteps = [(1, 1), (1, -1), (-1, -1), (-1, 1)];

    private static readonly (int File, int Rank)[] RookSteps = [(1, 0), (-1, 0), (0, 1), (0, -1)];

    private static readonly (int File, int Rank)[] KingSteps =
    [
        (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1),
    ];

    /// <summary>Every legal move in the position, in no particular order.</summary>
    public static List<Move> LegalMoves(Position position)
    {
        ArgumentNullException.ThrowIfNull(position);
        var moves = new List<Move>(48);
        var mover = position.SideToMove;

        for (var square = 0; square < 64; square++)
        {
            var piece = position[square];
            if (piece is null || piece.Value.Color != mover)
            {
                continue;
            }

            switch (piece.Value.Type)
            {
                case PieceType.Pawn:
                    AddPawnMoves(position, square, mover, moves);
                    break;
                case PieceType.Knight:
                    AddStepMoves(position, square, mover, KnightSteps, moves);
                    break;
                case PieceType.Bishop:
                    AddSlidingMoves(position, square, mover, BishopSteps, moves);
                    break;
                case PieceType.Rook:
                    AddSlidingMoves(position, square, mover, RookSteps, moves);
                    break;
                case PieceType.Queen:
                    AddSlidingMoves(position, square, mover, BishopSteps, moves);
                    AddSlidingMoves(position, square, mover, RookSteps, moves);
                    break;
                case PieceType.King:
                    AddStepMoves(position, square, mover, KingSteps, moves);
                    AddCastlingMoves(position, square, mover, moves);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(position));
            }
        }

        return [.. moves.Where(move => !LeavesKingAttacked(position, move))];
    }

    /// <summary>True when any of <paramref name="byColor"/>'s pieces attacks the square.</summary>
    public static bool IsSquareAttacked(Position position, int square, Color byColor)
    {
        var pawnRank = Squares.RankOf(square) + (byColor == Color.White ? -1 : 1);
        if (pawnRank is >= 0 and <= 7)
        {
            foreach (var fileOffset in (int[])[-1, 1])
            {
                var file = Squares.FileOf(square) + fileOffset;
                if (file is >= 0 and <= 7
                    && position[Squares.Index(file, pawnRank)] == new Piece(byColor, PieceType.Pawn))
                {
                    return true;
                }
            }
        }

        foreach (var (file, rank) in KnightSteps)
        {
            var fileTo = Squares.FileOf(square) + file;
            var rankTo = Squares.RankOf(square) + rank;
            if (fileTo is >= 0 and <= 7 && rankTo is >= 0 and <= 7
                && position[Squares.Index(fileTo, rankTo)] == new Piece(byColor, PieceType.Knight))
            {
                return true;
            }
        }

        foreach (var (file, rank) in KingSteps)
        {
            var fileTo = Squares.FileOf(square) + file;
            var rankTo = Squares.RankOf(square) + rank;
            if (fileTo is >= 0 and <= 7 && rankTo is >= 0 and <= 7
                && position[Squares.Index(fileTo, rankTo)] == new Piece(byColor, PieceType.King))
            {
                return true;
            }
        }

        foreach (var (file, rank) in BishopSteps)
        {
            if (RayHits(position, square, file, rank, byColor, PieceType.Bishop))
            {
                return true;
            }
        }

        foreach (var (file, rank) in RookSteps)
        {
            if (RayHits(position, square, file, rank, byColor, PieceType.Rook))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>True when the side to move is in check.</summary>
    public static bool IsInCheck(Position position)
    {
        var king = position.KingSquare(position.SideToMove);
        return king != Squares.None && IsSquareAttacked(position, king, Opponent(position.SideToMove));
    }

    private static Color Opponent(Color color) => color == Color.White ? Color.Black : Color.White;

    private static bool RayHits(Position position, int square, int fileStep, int rankStep, Color byColor, PieceType slider)
    {
        var file = Squares.FileOf(square) + fileStep;
        var rank = Squares.RankOf(square) + rankStep;
        while (file is >= 0 and <= 7 && rank is >= 0 and <= 7)
        {
            var candidate = position[Squares.Index(file, rank)];
            if (candidate is not null)
            {
                var piece = candidate.Value;
                return piece.Color == byColor && (piece.Type == slider || piece.Type == PieceType.Queen);
            }

            file += fileStep;
            rank += rankStep;
        }

        return false;
    }

    private static bool LeavesKingAttacked(Position position, Move move)
    {
        var mover = position.SideToMove;
        var next = position.ApplyUnchecked(move);
        var king = next.KingSquare(mover);
        return king != Squares.None && IsSquareAttacked(next, king, Opponent(mover));
    }

    private static void AddStepMoves(Position position, int from, Color mover, (int File, int Rank)[] steps, List<Move> moves)
    {
        foreach (var (file, rank) in steps)
        {
            var fileTo = Squares.FileOf(from) + file;
            var rankTo = Squares.RankOf(from) + rank;
            if (fileTo is < 0 or > 7 || rankTo is < 0 or > 7)
            {
                continue;
            }

            var to = Squares.Index(fileTo, rankTo);
            var target = position[to];
            if (target is null || target.Value.Color != mover)
            {
                moves.Add(new Move(from, to));
            }
        }
    }

    private static void AddSlidingMoves(Position position, int from, Color mover, (int File, int Rank)[] steps, List<Move> moves)
    {
        foreach (var (fileStep, rankStep) in steps)
        {
            var file = Squares.FileOf(from) + fileStep;
            var rank = Squares.RankOf(from) + rankStep;
            while (file is >= 0 and <= 7 && rank is >= 0 and <= 7)
            {
                var to = Squares.Index(file, rank);
                var target = position[to];
                if (target is null)
                {
                    moves.Add(new Move(from, to));
                }
                else
                {
                    if (target.Value.Color != mover)
                    {
                        moves.Add(new Move(from, to));
                    }

                    break;
                }

                file += fileStep;
                rank += rankStep;
            }
        }
    }

    private static void AddPawnMoves(Position position, int from, Color mover, List<Move> moves)
    {
        var file = Squares.FileOf(from);
        var rank = Squares.RankOf(from);
        var direction = mover == Color.White ? 1 : -1;
        var startRank = mover == Color.White ? 1 : 6;
        var lastRank = mover == Color.White ? 7 : 0;

        var oneAhead = rank + direction;
        if (oneAhead is >= 0 and <= 7 && position[Squares.Index(file, oneAhead)] is null)
        {
            AddPawnMove(moves, from, Squares.Index(file, oneAhead), lastRank, oneAhead);

            var twoAhead = rank + (2 * direction);
            if (rank == startRank && position[Squares.Index(file, twoAhead)] is null)
            {
                moves.Add(new Move(from, Squares.Index(file, twoAhead)));
            }
        }

        foreach (var fileStep in (int[])[-1, 1])
        {
            var fileTo = file + fileStep;
            var rankTo = rank + direction;
            if (fileTo is < 0 or > 7 || rankTo is < 0 or > 7)
            {
                continue;
            }

            var to = Squares.Index(fileTo, rankTo);
            var target = position[to];
            var isCapture = target is not null && target.Value.Color != mover;
            var isEnPassant = to == position.EnPassantSquare;
            if (isCapture || isEnPassant)
            {
                AddPawnMove(moves, from, to, lastRank, rankTo);
            }
        }
    }

    private static void AddPawnMove(List<Move> moves, int from, int to, int lastRank, int rankTo)
    {
        if (rankTo == lastRank)
        {
            foreach (var promotion in (PieceType[])[PieceType.Queen, PieceType.Rook, PieceType.Bishop, PieceType.Knight])
            {
                moves.Add(new Move(from, to, promotion));
            }
        }
        else
        {
            moves.Add(new Move(from, to));
        }
    }

    private static void AddCastlingMoves(Position position, int from, Color mover, List<Move> moves)
    {
        var rank = mover == Color.White ? 0 : 7;
        if (from != Squares.Index(4, rank) || IsSquareAttacked(position, from, Opponent(mover)))
        {
            return;
        }

        var kingSide = mover == Color.White ? CastlingRights.WhiteKingSide : CastlingRights.BlackKingSide;
        var queenSide = mover == Color.White ? CastlingRights.WhiteQueenSide : CastlingRights.BlackQueenSide;
        var opponent = Opponent(mover);

        if (position.Castling.HasFlag(kingSide)
            && position[Squares.Index(5, rank)] is null
            && position[Squares.Index(6, rank)] is null
            && !IsSquareAttacked(position, Squares.Index(5, rank), opponent)
            && !IsSquareAttacked(position, Squares.Index(6, rank), opponent))
        {
            moves.Add(new Move(from, Squares.Index(6, rank)));
        }

        if (position.Castling.HasFlag(queenSide)
            && position[Squares.Index(1, rank)] is null
            && position[Squares.Index(2, rank)] is null
            && position[Squares.Index(3, rank)] is null
            && !IsSquareAttacked(position, Squares.Index(3, rank), opponent)
            && !IsSquareAttacked(position, Squares.Index(2, rank), opponent))
        {
            moves.Add(new Move(from, Squares.Index(2, rank)));
        }
    }
}