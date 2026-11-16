namespace BlzChess.Core;

/// <summary>Which side a piece belongs to.</summary>
public enum Color
{
    White,
    Black,
}

/// <summary>What a piece is. No piece is also a value: a square is empty.</summary>
public enum PieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King,
}

/// <summary>A piece on the board: a color and a type.</summary>
public readonly record struct Piece(Color Color, PieceType Type)
{
    public Color Opponent => Color == Color.White ? Color.Black : Color.White;

    public static Piece? For(Color color, PieceType type) => new Piece(color, type);

    /// <summary>The FEN letter for this piece, uppercase for White.</summary>
    public char ToFenChar() => Type switch
    {
        PieceType.Pawn => Color == Color.White ? 'P' : 'p',
        PieceType.Knight => Color == Color.White ? 'N' : 'n',
        PieceType.Bishop => Color == Color.White ? 'B' : 'b',
        PieceType.Rook => Color == Color.White ? 'R' : 'r',
        PieceType.Queen => Color == Color.White ? 'Q' : 'q',
        PieceType.King => Color == Color.White ? 'K' : 'k',
        _ => throw new ArgumentOutOfRangeException(nameof(Type)),
    };
}