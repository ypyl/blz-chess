using BlzChess.Core;

namespace BlzChess.Core.Tests;

/// <summary>
/// The material count on the trays. Arithmetic over the list the trays already show, so
/// the tests are about the scale and about the list staying honest under a take-back.
/// </summary>
public class MaterialTests
{
    private static int Sq(string name)
    {
        Assert.True(Squares.TryParse(name, out var square));
        return square;
    }

    [Theory]
    [InlineData(PieceType.Pawn, 1)]
    [InlineData(PieceType.Knight, 3)]
    [InlineData(PieceType.Bishop, 3)]
    [InlineData(PieceType.Rook, 5)]
    [InlineData(PieceType.Queen, 9)]
    [InlineData(PieceType.King, 0)]
    public void TheScaleIsTheOneChessPlayersCount(PieceType type, int value)
    {
        Assert.Equal(value, Material.ValueOf(type));
    }

    [Fact(DisplayName = "Each side shows the value of what it has taken")]
    public void EachSideShowsTheValueOfWhatItHasTaken()
    {
        // 1. e4 d5 2. exd5 Qxd5 3. Nc3 Nf6 4. Nxd5 Nxd5: white took a pawn and a queen,
        // black took a pawn and a knight.
        var game = new Game();
        foreach (var (from, to) in new[]
        {
            ("e2", "e4"), ("d7", "d5"), ("e4", "d5"), ("d8", "d5"),
            ("b1", "c3"), ("g8", "f6"), ("c3", "d5"), ("f6", "d5"),
        })
        {
            Assert.True(game.TryMove(new Move(Sq(from), Sq(to)), out _));
        }

        Assert.Equal(10, Material.Taken(game, Color.Black));
        Assert.Equal(4, Material.Taken(game, Color.White));
    }

    [Fact(DisplayName = "A side that has taken nothing shows no value")]
    public void ASideThatHasTakenNothingShowsNoValue()
    {
        var game = new Game();

        Assert.Equal(0, Material.Taken(game, Color.White));
        Assert.Equal(0, Material.Taken(game, Color.Black));
    }

    [Fact(DisplayName = "The value follows a take-back")]
    public void TheValueFollowsATakeBack()
    {
        var game = new Game();
        Assert.True(game.TryMove(new Move(Sq("e2"), Sq("e4")), out _));
        Assert.True(game.TryMove(new Move(Sq("d7"), Sq("d5")), out _));
        Assert.True(game.TryMove(new Move(Sq("e4"), Sq("d5")), out _));
        Assert.Equal(1, Material.Taken(game, Color.Black));

        // A queen this time, so the drop is 9 and not 1.
        Assert.True(game.TryMove(new Move(Sq("d8"), Sq("d5")), out _));
        Assert.Equal(1, Material.Taken(game, Color.White));
        Assert.Equal(1, Material.Taken(game, Color.Black));

        Assert.True(game.TrimMoves(1));

        Assert.Equal(0, Material.Taken(game, Color.White));
        Assert.Equal(1, Material.Taken(game, Color.Black));
    }

    [Fact(DisplayName = "A captured promoted queen is worth a queen")]
    public void ACapturedPromotedQueenIsWorthAQueen()
    {
        // b7 takes the rook on a8 and promotes to a queen; the rook on a4 takes the queen.
        var game = new Game(Position.FromFen("r6k/1P6/8/8/r7/8/8/4K3 w - - 0 1"));
        Assert.True(game.TryMove(new Move(Sq("b7"), Sq("a8"), PieceType.Queen), out _));
        Assert.Equal(5, Material.Taken(game, Color.Black));

        Assert.True(game.TryMove(new Move(Sq("a4"), Sq("a8")), out _));

        Assert.Equal(9, Material.Taken(game, Color.White));
    }
}