using BlzChess.Core;

namespace BlzChess.Core.Tests;

public class PieceFilesTests
{
    [Theory]
    [InlineData(Color.White, PieceType.Pawn, "wP.svg")]
    [InlineData(Color.White, PieceType.Knight, "wN.svg")]
    [InlineData(Color.White, PieceType.Bishop, "wB.svg")]
    [InlineData(Color.White, PieceType.Rook, "wR.svg")]
    [InlineData(Color.White, PieceType.Queen, "wQ.svg")]
    [InlineData(Color.White, PieceType.King, "wK.svg")]
    [InlineData(Color.Black, PieceType.Knight, "bN.svg")]
    [InlineData(Color.Black, PieceType.King, "bK.svg")]
    public void EachPieceHasItsOwnFile(Color color, PieceType type, string expected)
    {
        Assert.Equal(expected, PieceFiles.FileName(new Piece(color, type)));
    }

    [Fact]
    public void TwelvePiecesTwelveFiles()
    {
        var files = Enum.GetValues<Color>()
            .SelectMany(color => Enum.GetValues<PieceType>().Select(type => PieceFiles.FileName(new Piece(color, type))))
            .ToList();

        Assert.Equal(12, files.Count);
        Assert.Equal(12, files.Distinct().Count());
    }
}