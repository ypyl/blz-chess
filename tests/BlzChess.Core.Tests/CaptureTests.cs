using BlzChess.Core;

namespace BlzChess.Core.Tests;

/// <summary>
/// What a game took, and when. The list is the page's tray, so it has to survive a
/// take-back exactly and has to know the difference between a captured queen and the
/// pawn that became one.
/// </summary>
public class CaptureTests
{
    private static int Sq(string name)
    {
        Assert.True(Squares.TryParse(name, out var square));
        return square;
    }

    [Fact(DisplayName = "No taken piece is shown before the first capture")]
    public void NoTakenPieceIsShownBeforeTheFirstCapture()
    {
        var game = new Game();

        Assert.Empty(game.Captured);
    }

    [Fact(DisplayName = "A taken piece appears beside the board on the side that took it")]
    public void ATakenPieceAppearsOnTheSideThatTookIt()
    {
        // 1. e4 d5 2. exd5 takes a black pawn.
        var game = new Game();
        Assert.True(game.TryMove(new Move(Sq("e2"), Sq("e4")), out _));
        Assert.True(game.TryMove(new Move(Sq("d7"), Sq("d5")), out _));
        Assert.True(game.TryMove(new Move(Sq("e4"), Sq("d5")), out _));

        var taken = Assert.Single(game.Captured);
        Assert.Equal(new Piece(Color.Black, PieceType.Pawn), taken);
        Assert.Equal(new Piece(Color.White, PieceType.Pawn), game.Position[Sq("d5")]);
    }

    [Fact(DisplayName = "A piece taken in passing leaves the board")]
    public void APieceTakenInPassingLeavesTheBoard()
    {
        // White pawn to e5, black pawn to d5 beside it, then exd6 takes in passing: the
        // taken pawn stands on d5, and nothing moves onto that square.
        var game = new Game();
        Assert.True(game.TryMove(new Move(Sq("e2"), Sq("e4")), out _));
        Assert.True(game.TryMove(new Move(Sq("a7"), Sq("a6")), out _));
        Assert.True(game.TryMove(new Move(Sq("e4"), Sq("e5")), out _));
        Assert.True(game.TryMove(new Move(Sq("d7"), Sq("d5")), out _));
        Assert.True(game.TryMove(new Move(Sq("e5"), Sq("d6")), out _));

        var taken = Assert.Single(game.Captured);
        Assert.Equal(new Piece(Color.Black, PieceType.Pawn), taken);
        Assert.Null(game.Position[Sq("d5")]);
        Assert.Equal(new Piece(Color.White, PieceType.Pawn), game.Position[Sq("d6")]);
    }

    [Fact(DisplayName = "Taking the move back returns the piece to the board")]
    public void TakingTheMoveBackReturnsThePieceToTheBoard()
    {
        var game = new Game();
        Assert.True(game.TryMove(new Move(Sq("e2"), Sq("e4")), out _));
        Assert.True(game.TryMove(new Move(Sq("d7"), Sq("d5")), out _));
        Assert.True(game.TryMove(new Move(Sq("e4"), Sq("d5")), out _));
        Assert.Single(game.Captured);

        Assert.True(game.TrimMoves(1));

        Assert.Empty(game.Captured);
        Assert.Equal(new Piece(Color.Black, PieceType.Pawn), game.Position[Sq("d5")]);
        Assert.Equal(new Piece(Color.White, PieceType.Pawn), game.Position[Sq("e4")]);
    }

    [Fact(DisplayName = "A captured piece is shown as the piece it had become")]
    public void ACapturedPieceIsShownAsThePieceItHadBecome()
    {
        // A white pawn on b7 promotes by taking the rook on a8, and the black rook on a4
        // then takes that queen. Counting the board and comparing with the opening would
        // report a pawn here, because a pawn is missing and a queen is missing: the pawn
        // that turned into the queen is long gone and a queen never started on the board.
        var game = new Game(Position.FromFen("r6k/1P6/8/8/r7/8/8/4K3 w - - 0 1"));

        Assert.True(game.TryMove(new Move(Sq("b7"), Sq("a8"), PieceType.Queen), out _));
        Assert.Equal(new Piece(Color.White, PieceType.Queen), game.Position[Sq("a8")]);
        Assert.Equal(new Piece(Color.Black, PieceType.Rook), Assert.Single(game.Captured));

        Assert.True(game.TryMove(new Move(Sq("a4"), Sq("a8")), out _));

        Assert.Equal(new Piece(Color.Black, PieceType.Rook), game.Captured[0]);
        Assert.Equal(new Piece(Color.White, PieceType.Queen), game.Captured[1]);
        Assert.DoesNotContain(new Piece(Color.White, PieceType.Pawn), game.Captured);
    }
}