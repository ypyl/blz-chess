using BlzChess.Core;

namespace BlzChess.Core.Tests;

public class FenTests
{
    private const string Kiwipete = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";

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

    [Theory]
    [InlineData(Fen.StartingPosition)]
    [InlineData(Kiwipete)]
    [InlineData("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1")]
    [InlineData("rnbq1k1r/pp1Pbppp/2p5/8/2B5/8/PPP1NnPP/RNBQK2R w KQ - 1 8")]
    public void FenRoundTripKeepsEveryField(string fen)
    {
        var position = Position.FromFen(fen);

        Assert.Equal(fen, position.ToFen());
    }

    [Fact]
    public void PlacementKeyDropsTheClocks()
    {
        var position = Position.FromFen("8/8/4k3/8/8/4K3/8/6B1 w - - 12 40");

        Assert.Equal("8/8/4k3/8/8/4K3/8/6B1 w - -", position.PlacementKey());
    }
}