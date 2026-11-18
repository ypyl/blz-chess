using BlzChess.Core;

namespace BlzChess.Core.Tests;

/// <summary>
/// Dragging: which square the pointer is over, and what that square means for a piece
/// already lifted. The browser wiring is proven with Playwright; these tests own the
/// arithmetic it leans on.
/// </summary>
public class DragTests
{
    // Board geometry. The drawn board is square, side units wide, with rank 8 at the
    // top. Everything below is derived from that, not from the pixels of one window.
    private const double DesktopSide = 544;
    private const double PhoneSide = 222;

    private static (double X, double Y) Centre(int square, double side)
    {
        var cell = side / 8;
        return ((Squares.FileOf(square) * cell) + (cell / 2), ((7 - Squares.RankOf(square)) * cell) + (cell / 2));
    }

    [Theory]
    [InlineData(DesktopSide)]
    [InlineData(PhoneSide)]
    public void TheSquareUnderThePointerIsTheOneDrawnThere(double side)
    {
        foreach (var square in Enumerable.Range(0, 64))
        {
            var (x, y) = Centre(square, side);
            Assert.Equal(square, BoardGeometry.SquareAt(x, y, side));
        }
    }

    [Theory]
    [InlineData(DesktopSide)]
    [InlineData(PhoneSide)]
    public void TheCornersAndEdgesBelongToTheSquaresDrawnThere(double side)
    {
        var cell = side / 8;

        // Top-left of the board is a8, bottom-right is h1, and the last pixel inside the
        // board still belongs to a square rather than falling off it.
        Assert.Equal(Squares.Index(0, 7), BoardGeometry.SquareAt(0, 0, side));
        Assert.Equal(Squares.Index(7, 0), BoardGeometry.SquareAt(side - 0.01, side - 0.01, side));
        Assert.Equal(Squares.Index(0, 0), BoardGeometry.SquareAt(0, side - 0.01, side));
        Assert.Equal(Squares.Index(7, 7), BoardGeometry.SquareAt(side - 0.01, 0, side));

        // A point just inside the boundary between a8 and b8 is b8.
        Assert.Equal(Squares.Index(1, 7), BoardGeometry.SquareAt(cell, 0, side));
        Assert.Equal(Squares.Index(0, 6), BoardGeometry.SquareAt(0, cell, side));
    }

    [Fact]
    public void APointOffTheBoardIsNoSquare()
    {
        Assert.Equal(Squares.None, BoardGeometry.SquareAt(-1, 10, DesktopSide));
        Assert.Equal(Squares.None, BoardGeometry.SquareAt(10, -1, DesktopSide));
        Assert.Equal(Squares.None, BoardGeometry.SquareAt(DesktopSide, 10, DesktopSide));
        Assert.Equal(Squares.None, BoardGeometry.SquareAt(10, DesktopSide, DesktopSide));
        Assert.Equal(Squares.None, BoardGeometry.SquareAt(double.NaN, 10, DesktopSide));
        Assert.Equal(Squares.None, BoardGeometry.SquareAt(10, 10, 0));
    }

    // The three outcomes a drag can have.

    [Fact(DisplayName = "A piece dragged to a legal square moves there")]
    public async Task APieceDraggedToALegalSquareMovesThere()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        var pawn = Squares.Index(4, 1);
        var e4 = Squares.Index(4, 3);

        // The press lifts the pawn, and the pointer travels to e4's centre.
        var lifted = Drag.Lift(flow.Session, pawn);
        var destinations = flow.Session.LegalMovesFrom(lifted).Select(move => move.To).ToHashSet();
        var landing = Drag.Land(destinations, BoardGeometry.SquareAt(Centre(e4, DesktopSide).X, Centre(e4, DesktopSide).Y, DesktopSide));

        Assert.Equal(pawn, lifted);
        Assert.Equal(e4, landing);
        Assert.True(flow.TryPlayerMove(new Move(lifted, landing), out var applied));
        Assert.Equal(new Move(pawn, e4), applied);
        Assert.Equal(new Piece(Color.White, PieceType.Pawn), flow.Session.Game.Position[e4]);
        Assert.Null(flow.Session.Game.Position[pawn]);
        Assert.Single(flow.Session.Game.Moves);
        Assert.NotNull(await flow.PlayEngineMoveAsync());
    }

    [Fact(DisplayName = "A piece dropped where it may not go returns to its square")]
    public void APieceDroppedWhereItMayNotGoReturnsToItsSquare()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        var pawn = Squares.Index(4, 1);
        var h6 = Squares.Index(7, 5);

        var lifted = Drag.Lift(flow.Session, pawn);
        var destinations = flow.Session.LegalMovesFrom(lifted).Select(move => move.To).ToHashSet();
        var landing = Drag.Land(destinations, BoardGeometry.SquareAt(Centre(h6, DesktopSide).X, Centre(h6, DesktopSide).Y, DesktopSide));

        Assert.Equal(Squares.None, landing);
        Assert.Equal(new Piece(Color.White, PieceType.Pawn), flow.Session.Game.Position[pawn]);
        Assert.Null(flow.Session.Game.Position[h6]);
        Assert.Empty(flow.Session.Game.Moves);
        Assert.Equal(Color.White, flow.Session.Game.Position.SideToMove);
    }

    [Fact(DisplayName = "A drag on the computer's piece lifts nothing")]
    public void ADragOnTheComputersPieceLiftsNothing()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));

        Assert.Equal(Squares.None, Drag.Lift(flow.Session, Squares.Index(4, 6)));
        Assert.Equal(Squares.None, Drag.Lift(flow.Session, Squares.Index(0, 7)));
        Assert.Equal(Squares.None, Drag.Lift(flow.Session, Squares.Index(4, 4)));
    }

    [Fact(DisplayName = "A drag on a piece that cannot move lifts nothing")]
    public void ADragOnAPieceThatCannotMoveLiftsNothing()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));

        // A rook in the opening corner has no square to go to.
        Assert.Equal(Squares.None, Drag.Lift(flow.Session, Squares.Index(0, 0)));
    }

    [Fact(DisplayName = "A tap still moves a piece without dragging")]
    public async Task ATapStillMovesAPieceWithoutDragging()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        var pawn = Squares.Index(4, 1);
        var e4 = Squares.Index(4, 3);

        // The click path: select the piece, then its destination. No press threshold and
        // no lifted piece, and the same move lands.
        var destinations = flow.Session.LegalMovesFrom(pawn).Select(move => move.To).ToHashSet();
        Assert.Contains(e4, destinations);
        Assert.True(flow.TryPlayerMove(new Move(pawn, e4), out _));
        Assert.Equal(new Piece(Color.White, PieceType.Pawn), flow.Session.Game.Position[e4]);
        Assert.NotNull(await flow.PlayEngineMoveAsync());
    }

    [Fact(DisplayName = "A drag is refused while the computer is thinking")]
    public async Task ADragIsRefusedWhileTheComputerIsThinking()
    {
        // The strongest level always asks the engine, so "thinking" can be held open;
        // the weakest sometimes answers from its own list and never waits.
        var engine = new SlowEngine();
        var flow = new GameFlow(engine, Level.Strongest);

        // The computer only moves after the player, so the turn has to be handed over.
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        var reply = flow.PlayEngineMoveAsync();

        Assert.True(flow.Session.IsEngineThinking);

        // The player's own pawn, on the player's own turn by the clock, lifts nothing
        // while the answer is on its way.
        Assert.Equal(Squares.None, Drag.Lift(flow.Session, Squares.Index(3, 1)));

        engine.Release.TrySetResult(null);
        await reply;
    }
}

/// <summary>An engine that answers only when the test says so, so "thinking" can be held.</summary>
internal sealed class SlowEngine : IEngine
{
    public TaskCompletionSource<Move?> Release { get; } = new();

    public Task<Move?> BestMoveAsync(string fen, Level level, CancellationToken cancellationToken = default) =>
        Release.Task;
}