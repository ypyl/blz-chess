namespace BlzChess.Core;

/// <summary>Which castles are still allowed, one bit per side and color.</summary>
[Flags]
public enum CastlingRights
{
    None = 0,
    WhiteKingSide = 1,
    WhiteQueenSide = 2,
    BlackKingSide = 4,
    BlackQueenSide = 8,
}

/// <summary>
/// A chess position: pieces, side to move, and the four facts FEN carries. Every
/// method that changes the position returns a new instance, so a move can be tried
/// and thrown away.
/// </summary>
public sealed class Position
{
    private readonly Piece?[] _board = new Piece?[64];

    private Position()
    {
    }

    public static Position Initial => FromFen(Fen.StartingPosition);

    public Color SideToMove { get; private set; }

    public CastlingRights Castling { get; private set; }

    /// <summary>The square a pawn may capture onto, or <see cref="Squares.None"/>.</summary>
    public int EnPassantSquare { get; private set; } = Squares.None;

    public int HalfmoveClock { get; private set; }

    public int FullmoveNumber { get; private set; } = 1;

    public Piece? this[int square] => _board[square];

    public Position Clone()
    {
        var copy = new Position
        {
            SideToMove = SideToMove,
            Castling = Castling,
            EnPassantSquare = EnPassantSquare,
            HalfmoveClock = HalfmoveClock,
            FullmoveNumber = FullmoveNumber,
        };
        Array.Copy(_board, copy._board, 64);
        return copy;
    }

    public static Position FromFen(string fen)
    {
        ArgumentNullException.ThrowIfNull(fen);
        var fields = fen.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (fields.Length < 4)
        {
            throw new ArgumentException($"Not a FEN with at least four fields: '{fen}'", nameof(fen));
        }

        var position = new Position();
        var rows = fields[0].Split('/');
        if (rows.Length != 8)
        {
            throw new ArgumentException($"Not eight ranks: '{fields[0]}'", nameof(fen));
        }

        for (var row = 0; row < 8; row++)
        {
            var rank = 7 - row;
            var file = 0;
            foreach (var symbol in rows[row])
            {
                if (char.IsAsciiDigit(symbol))
                {
                    file += symbol - '0';
                    continue;
                }

                if (file > 7)
                {
                    throw new ArgumentException($"Rank '{rows[row]}' overflows the board", nameof(fen));
                }

                position._board[Squares.Index(file, rank)] = PieceFromFenChar(symbol);
                file++;
            }

            if (file != 8)
            {
                throw new ArgumentException($"Rank '{rows[row]}' does not fill eight files", nameof(fen));
            }
        }

        position.SideToMove = fields[1] switch
        {
            "w" => Color.White,
            "b" => Color.Black,
            _ => throw new ArgumentException($"Side to move is '{fields[1]}'", nameof(fen)),
        };

        position.Castling = CastlingRights.None;
        foreach (var symbol in fields[2])
        {
            position.Castling |= symbol switch
            {
                'K' => CastlingRights.WhiteKingSide,
                'Q' => CastlingRights.WhiteQueenSide,
                'k' => CastlingRights.BlackKingSide,
                'q' => CastlingRights.BlackQueenSide,
                '-' => CastlingRights.None,
                _ => throw new ArgumentException($"Castling field is '{fields[2]}'", nameof(fen)),
            };
        }

        position.EnPassantSquare = fields[3] == "-"
            ? Squares.None
            : Squares.TryParse(fields[3], out var enPassant)
                ? enPassant
                : throw new ArgumentException($"En passant field is '{fields[3]}'", nameof(fen));

        position.HalfmoveClock = fields.Length > 4 && int.TryParse(fields[4], out var halfmove) ? halfmove : 0;
        position.FullmoveNumber = fields.Length > 5 && int.TryParse(fields[5], out var fullmove) ? fullmove : 1;
        return position;
    }

    public string ToFen()
    {
        var builder = new System.Text.StringBuilder();
        for (var rank = 7; rank >= 0; rank--)
        {
            var empty = 0;
            for (var file = 0; file < 8; file++)
            {
                var piece = _board[Squares.Index(file, rank)];
                if (piece is null)
                {
                    empty++;
                    continue;
                }

                if (empty > 0)
                {
                    builder.Append(empty);
                    empty = 0;
                }

                builder.Append(piece.Value.ToFenChar());
            }

            if (empty > 0)
            {
                builder.Append(empty);
            }

            if (rank > 0)
            {
                builder.Append('/');
            }
        }

        builder.Append(' ').Append(SideToMove == Color.White ? 'w' : 'b');
        builder.Append(' ');
        if (Castling == CastlingRights.None)
        {
            builder.Append('-');
        }
        else
        {
            if (Castling.HasFlag(CastlingRights.WhiteKingSide))
            {
                builder.Append('K');
            }

            if (Castling.HasFlag(CastlingRights.WhiteQueenSide))
            {
                builder.Append('Q');
            }

            if (Castling.HasFlag(CastlingRights.BlackKingSide))
            {
                builder.Append('k');
            }

            if (Castling.HasFlag(CastlingRights.BlackQueenSide))
            {
                builder.Append('q');
            }
        }

        builder.Append(' ').Append(Squares.Name(EnPassantSquare));
        builder.Append(' ').Append(HalfmoveClock);
        builder.Append(' ').Append(FullmoveNumber);
        return builder.ToString();
    }

    /// <summary>
    /// The position without its clocks, which is what makes two positions the same
    /// for repetition: same pieces, same side to move, same rights, same en passant.
    /// </summary>
    public string PlacementKey()
    {
        var fen = ToFen();
        var lastSpace = fen.LastIndexOf(' ');
        var second = fen.LastIndexOf(' ', lastSpace - 1);
        return fen[..second];
    }

    /// <summary>The square of a color's king, or <see cref="Squares.None"/> in a broken position.</summary>
    public int KingSquare(Color color)
    {
        for (var square = 0; square < 64; square++)
        {
            if (_board[square] == new Piece(color, PieceType.King))
            {
                return square;
            }
        }

        return Squares.None;
    }

    public int PieceCount()
    {
        var count = 0;
        foreach (var piece in _board)
        {
            if (piece is not null)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Applies a move with no legality check: callers that accept player or engine
    /// input go through <see cref="Game"/> or <see cref="MoveGenerator"/>. This is
    /// also how legality is decided: try the move, then ask whether the mover's king
    /// is attacked.
    /// </summary>
    public Position ApplyUnchecked(Move move)
    {
        var next = Clone();
        var mover = SideToMove;
        var piece = _board[move.From] ?? throw new InvalidOperationException($"No piece on {Squares.Name(move.From)}");
        if (piece.Color != mover)
        {
            throw new InvalidOperationException($"{Squares.Name(move.From)} holds a {piece.Color} piece, not the side to move");
        }

        var target = next._board[move.To];
        var isCapture = target is not null;

        next._board[move.From] = null;

        if (piece.Type == PieceType.Pawn
            && move.To == EnPassantSquare
            && Squares.FileOf(move.From) != Squares.FileOf(move.To)
            && target is null)
        {
            var capturedSquare = move.To + (mover == Color.White ? -8 : 8);
            next._board[capturedSquare] = null;
            isCapture = true;
        }

        next._board[move.To] = move.Promotion is { } promotion
            ? new Piece(mover, promotion)
            : piece;

        if (piece.Type == PieceType.King && Math.Abs(Squares.FileOf(move.To) - Squares.FileOf(move.From)) == 2)
        {
            var rank = Squares.RankOf(move.From);
            var kingSide = Squares.FileOf(move.To) == 6;
            var rookFrom = Squares.Index(kingSide ? 7 : 0, rank);
            var rookTo = Squares.Index(kingSide ? 5 : 3, rank);
            next._board[rookTo] = next._board[rookFrom];
            next._board[rookFrom] = null;
        }

        next.Castling = UpdateCastling(Castling, piece, move);
        next.EnPassantSquare = piece.Type == PieceType.Pawn
            && Math.Abs(Squares.RankOf(move.To) - Squares.RankOf(move.From)) == 2
                ? (move.From + move.To) / 2
                : Squares.None;

        next.HalfmoveClock = piece.Type == PieceType.Pawn || isCapture ? 0 : HalfmoveClock + 1;
        next.FullmoveNumber = FullmoveNumber + (mover == Color.Black ? 1 : 0);
        next.SideToMove = mover == Color.White ? Color.Black : Color.White;
        return next;
    }

    private static CastlingRights UpdateCastling(CastlingRights rights, Piece piece, Move move)
    {
        if (piece.Type == PieceType.King)
        {
            return rights & (piece.Color == Color.White
                ? ~(CastlingRights.WhiteKingSide | CastlingRights.WhiteQueenSide)
                : ~(CastlingRights.BlackKingSide | CastlingRights.BlackQueenSide));
        }

        if (piece.Type == PieceType.Rook)
        {
            rights = RemoveCornerRights(rights, move.From);
        }

        return RemoveCornerRights(rights, move.To);
    }

    private static CastlingRights RemoveCornerRights(CastlingRights rights, int square)
    {
        if (square == Squares.Index(0, 0))
        {
            return rights & ~CastlingRights.WhiteQueenSide;
        }

        if (square == Squares.Index(7, 0))
        {
            return rights & ~CastlingRights.WhiteKingSide;
        }

        if (square == Squares.Index(0, 7))
        {
            return rights & ~CastlingRights.BlackQueenSide;
        }

        return square == Squares.Index(7, 7) ? rights & ~CastlingRights.BlackKingSide : rights;
    }

    private static Piece PieceFromFenChar(char symbol) => symbol switch
    {
        'P' => new Piece(Color.White, PieceType.Pawn),
        'N' => new Piece(Color.White, PieceType.Knight),
        'B' => new Piece(Color.White, PieceType.Bishop),
        'R' => new Piece(Color.White, PieceType.Rook),
        'Q' => new Piece(Color.White, PieceType.Queen),
        'K' => new Piece(Color.White, PieceType.King),
        'p' => new Piece(Color.Black, PieceType.Pawn),
        'n' => new Piece(Color.Black, PieceType.Knight),
        'b' => new Piece(Color.Black, PieceType.Bishop),
        'r' => new Piece(Color.Black, PieceType.Rook),
        'q' => new Piece(Color.Black, PieceType.Queen),
        'k' => new Piece(Color.Black, PieceType.King),
        _ => throw new ArgumentException($"'{symbol}' is not a piece"),
    };
}