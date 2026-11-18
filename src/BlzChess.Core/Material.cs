namespace BlzChess.Core;

/// <summary>
/// What a piece is worth, as a chess player counts it, and what a side has taken off the
/// board. One place, so the page adds nothing up itself.
/// </summary>
public static class Material
{
    /// <summary>
    /// The usual scale. A king is worth nothing because a king is never taken, and a tray
    /// can never hold one.
    /// </summary>
    public static int ValueOf(PieceType type) => type switch
    {
        PieceType.Pawn => 1,
        PieceType.Knight => 3,
        PieceType.Bishop => 3,
        PieceType.Rook => 5,
        PieceType.Queen => 9,
        _ => 0,
    };

    /// <summary>The total value of the pieces of <paramref name="color"/> that are off the board.</summary>
    public static int Taken(Game game, Color color)
    {
        ArgumentNullException.ThrowIfNull(game);
        return game.Captured.Where(piece => piece.Color == color).Sum(piece => ValueOf(piece.Type));
    }
}