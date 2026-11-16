namespace BlzChess.Core;

/// <summary>
/// One move, from a square to a square. <paramref name="Promotion"/> is set only
/// when a pawn reaches the last rank.
/// </summary>
public readonly record struct Move(int From, int To, PieceType? Promotion = null)
{
    /// <summary>Long algebraic notation for one move, as UCI writes it: <c>e2e4</c>, <c>e7e8q</c>.</summary>
    public string ToUci()
    {
        var suffix = Promotion switch
        {
            PieceType.Queen => "q",
            PieceType.Rook => "r",
            PieceType.Bishop => "b",
            PieceType.Knight => "n",
            _ => string.Empty,
        };

        return Squares.Name(From) + Squares.Name(To) + suffix;
    }

    /// <summary>Parses UCI move text, the form Stockfish answers with.</summary>
    public static bool TryParseUci(string text, out Move move)
    {
        move = default;
        if (text is null || text.Length is < 4 or > 5)
        {
            return false;
        }

        if (!Squares.TryParse(text[..2], out var from) || !Squares.TryParse(text[2..4], out var to))
        {
            return false;
        }

        PieceType? promotion = text.Length == 5
            ? char.ToLowerInvariant(text[4]) switch
            {
                'q' => PieceType.Queen,
                'r' => PieceType.Rook,
                'b' => PieceType.Bishop,
                'n' => PieceType.Knight,
                _ => null,
            }
            : null;

        if (text.Length == 5 && promotion is null)
        {
            return false;
        }

        move = new Move(from, to, promotion);
        return true;
    }

    public override string ToString() => ToUci();
}

/// <summary>Square arithmetic: index 0 is a1, index 63 is h8, rank-major.</summary>
public static class Squares
{
    public const int None = -1;

    public static int Index(int file, int rank) => (rank * 8) + file;

    public static int FileOf(int square) => square & 7;

    public static int RankOf(int square) => square >> 3;

    public static bool IsLight(int square) => (FileOf(square) + RankOf(square)) % 2 == 1;

    public static string Name(int square) =>
        square == None ? "-" : $"{(char)('a' + FileOf(square))}{(char)('1' + RankOf(square))}";

    public static bool TryParse(string text, out int square)
    {
        square = None;
        if (text is null || text.Length != 2)
        {
            return false;
        }

        var file = char.ToLowerInvariant(text[0]) - 'a';
        var rank = text[1] - '1';
        if (file is < 0 or > 7 || rank is < 0 or > 7)
        {
            return false;
        }

        square = Index(file, rank);
        return true;
    }
}