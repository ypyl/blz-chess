namespace BlzChess.Core;

/// <summary>
/// Forsyth-Edwards Notation. This is the only thing the library claims to know
/// yet; move generation arrives with the first feature change that needs it.
/// </summary>
public static class Fen
{
    /// <summary>The position every game starts from.</summary>
    public const string StartingPosition =
        "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
}