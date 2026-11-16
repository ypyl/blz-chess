using BlzChess.Core;

namespace BlzChess.Core.Tests;

public class FenTests
{
    [Fact]
    public void StartPositionPiecesAndMoveMarker()
    {
        var fields = Fen.StartingPosition.Split(' ');

        Assert.Equal(6, fields.Length);
        Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", fields[0]);
        Assert.Equal("w", fields[1]);
        Assert.Equal("KQkq", fields[2]);
        Assert.Equal("-", fields[3]);
        Assert.Equal("0", fields[4]);
        Assert.Equal("1", fields[5]);
    }
}