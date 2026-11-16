using BlzChess.Core;

namespace BlzChess.Core.Tests;

public class GameFlowTests
{
    [Fact(DisplayName = "The page opens on the level list")]
    public void ThePageOpensOnTheLevelList()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));

        Assert.Equal(GameStage.Choosing, flow.Stage);
        Assert.Null(flow.Session);
        Assert.Equal(
            ["No idea how to play", "Just learning", "Beginner", "Casual", "Club player", "Expert"],
            Level.All.Select(level => level.Name));
    }

    [Fact(DisplayName = "Choosing a level starts a game")]
    public void ChoosingALevelStartsAGame()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));

        flow.ChooseLevel(Level.All[3]);

        Assert.Equal(GameStage.Playing, flow.Stage);
        Assert.Equal("Casual", flow.Level?.Name);
        Assert.Equal(Color.White, flow.Session?.Game.Position.SideToMove);
        Assert.Equal(Fen.StartingPosition, flow.Session?.Game.Fen);
    }

    [Fact(DisplayName = "A new game asks for a level again")]
    public async Task ANewGameAsksForALevelAgain()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        flow.ChooseLevel(Level.All[1]);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await flow.PlayEngineMoveAsync();

        flow.NewGame();

        Assert.Equal(GameStage.Choosing, flow.Stage);
        Assert.Null(flow.Session);
    }

    [Fact(DisplayName = "A refresh loses the game")]
    public void ARefreshLosesTheGame()
    {
        var first = new GameFlow(new FakeEngine("e7e5"));
        first.ChooseLevel(Level.All[3]);

        var afterRefresh = new GameFlow(new FakeEngine("e7e5"));

        Assert.Equal(GameStage.Playing, first.Stage);
        Assert.Equal(GameStage.Choosing, afterRefresh.Stage);
        Assert.Null(afterRefresh.Session);
    }

    [Fact]
    public async Task TheEngineMovesAfterThePlayerAndNotBefore()
    {
        var engine = new FakeEngine("e7e5");
        var flow = new GameFlow(engine);
        flow.ChooseLevel(Level.Strongest);

        Assert.Null(await flow.PlayEngineMoveAsync());
        Assert.Equal(0, engine.Calls);

        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.Equal(1, engine.Calls);
        Assert.Equal(Color.White, flow.Session?.Game.Position.SideToMove);
    }

    [Fact]
    public void ThePlayerCannotMoveWhileItIsTheComputersTurn()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        flow.ChooseLevel(Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));

        var blackMove = new Move(Squares.Index(4, 6), Squares.Index(4, 4));
        var refused = flow.TryPlayerMove(blackMove, out _);

        Assert.False(refused);
        Assert.Equal(Color.Black, flow.Session?.Game.Position.SideToMove);
    }

    [Fact]
    public async Task ACheckmatedGameFinishes()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        flow.ChooseLevel(Level.Strongest);

        foreach (var uci in new[] { "f2f3", "e7e5", "g2g4", "d8h4" })
        {
            if (flow.Session!.Game.Position.SideToMove == Color.White)
            {
                Assert.True(Move.TryParseUci(uci, out var playerMove));
                Assert.True(flow.TryPlayerMove(playerMove, out _));
            }
            else
            {
                Assert.True(Move.TryParseUci(uci, out var engineMove));
                Assert.True(flow.Session.Game.TryMove(engineMove, out _));
            }
        }

        Assert.Equal(ResultKind.Checkmate, flow.Session!.Result.Kind);
    }
}