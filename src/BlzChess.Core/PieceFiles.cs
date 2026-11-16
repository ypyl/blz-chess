namespace BlzChess.Core;

/// <summary>
/// The file name of a piece's image. One place, because the first letter of the
/// type name is not it: Knight and King both start with K, which is how every
/// knight once rendered as a king.
/// </summary>
public static class PieceFiles
{
    public const string Extension = ".svg";

    /// <summary>For example <c>wN.svg</c> or <c>bQ.svg</c>.</summary>
    public static string FileName(Piece piece) => $"{ColorLetter(piece.Color)}{TypeLetter(piece.Type)}{Extension}";

    public static char ColorLetter(Color color) => color == Color.White ? 'w' : 'b';

    public static char TypeLetter(PieceType type) => type switch
    {
        PieceType.Pawn => 'P',
        PieceType.Knight => 'N',
        PieceType.Bishop => 'B',
        PieceType.Rook => 'R',
        PieceType.Queen => 'Q',
        PieceType.King => 'K',
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "No piece file for that type"),
    };

    /// <summary>What a screen reader says, for one piece.</summary>
    public static string AltText(Piece piece) =>
        $"{(piece.Color == Color.White ? "white" : "black")} {piece.Type.ToString().ToLowerInvariant()}";
}