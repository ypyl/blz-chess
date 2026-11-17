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

    [Fact(DisplayName = "Restarting starts the same level again")]
    public async Task RestartingStartsTheSameLevelAgain()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        flow.ChooseLevel(Level.All[3]);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await flow.PlayEngineMoveAsync();
        var played = flow.Session!.Game.Moves.Count;

        var restarted = flow.Restart();

        Assert.True(restarted);
        Assert.Equal("Casual", flow.Level?.Name);
        Assert.Equal(GameStage.Playing, flow.Stage);
        Assert.Equal(Fen.StartingPosition, flow.Session?.Game.Fen);
        Assert.Equal(Color.White, flow.Session?.Game.Position.SideToMove);
        Assert.Empty(flow.Session!.Game.Moves);
        Assert.True(played > 0);
    }

    [Fact(DisplayName = "A restart records no result")]
    public async Task ARestartRecordsNoResult()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        flow.ChooseLevel(Level.All[3]);
        var before = flow.Session;
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await flow.PlayEngineMoveAsync();

        flow.Restart();

        Assert.NotSame(before, flow.Session);
        Assert.Equal(ResultKind.InProgress, flow.Result.Kind);
        Assert.Null(flow.Result.Winner);
        Assert.False(flow.Result.IsOver);
    }

    [Fact(DisplayName = "Restart is offered only while a game is in progress")]
    public async Task RestartIsOfferedOnlyWhileAGameIsInProgress()
    {
        var onTheLevelList = new GameFlow(new FakeEngine("e7e5"));
        Assert.False(onTheLevelList.Restart());

        // Fool's mate, driven through the flow so its stage moves as the game does.
        var finished = new GameFlow(new ScriptedEngine("e7e5", "d8h4"));
        finished.ChooseLevel(Level.Strongest);
        Assert.True(finished.TryPlayerMove(new Move(Squares.Index(5, 1), Squares.Index(5, 2)), out _));
        Assert.NotNull(await finished.PlayEngineMoveAsync());
        Assert.True(finished.TryPlayerMove(new Move(Squares.Index(6, 1), Squares.Index(6, 3)), out _));
        Assert.NotNull(await finished.PlayEngineMoveAsync());

        Assert.Equal(ResultKind.Checkmate, finished.Result.Kind);
        Assert.Equal(GameStage.Finished, finished.Stage);
        Assert.False(finished.Restart());
    }

    [Fact(DisplayName = "Changing the level keeps the position")]
    public async Task ChangingTheLevelKeepsThePosition()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        flow.ChooseLevel(Level.All[3]);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await flow.PlayEngineMoveAsync();
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(3, 1), Squares.Index(3, 3)), out _));

        var before = flow.Session!.Game.Fen;
        var moves = flow.Session.Game.Moves.Count;
        var toMove = flow.Session.Game.Position.SideToMove;

        var changed = flow.ChangeLevel(Level.Strongest);

        Assert.True(changed);
        Assert.Equal("Expert", flow.Level?.Name);
        Assert.Equal(before, flow.Session!.Game.Fen);
        Assert.Equal(moves, flow.Session.Game.Moves.Count);
        Assert.Equal(toMove, flow.Session.Game.Position.SideToMove);
        Assert.Equal(GameStage.Playing, flow.Stage);
    }

    [Fact(DisplayName = "The new level answers")]
    public async Task TheNewLevelAnswers()
    {
        var engine = new FakeEngine("e7e5");
        var flow = new GameFlow(engine);
        // A level with no random share, so the engine is the one asked every time.
        flow.ChooseLevel(Level.All[4]);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await flow.PlayEngineMoveAsync();
        Assert.Equal("Club player", engine.LastLevel?.Name);

        flow.ChangeLevel(Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(3, 1), Squares.Index(3, 3)), out _));
        await flow.PlayEngineMoveAsync();

        Assert.Equal("Expert", engine.LastLevel?.Name);
    }

    [Fact]
    public async Task ChangingTheLevelIsOfferedOnlyWhileAGameIsInProgress()
    {
        var onTheLevelList = new GameFlow(new FakeEngine("e7e5"));
        Assert.False(onTheLevelList.ChangeLevel(Level.Strongest));

        // Fool's mate through the flow, so the stage really reaches Finished.
        var finished = new GameFlow(new ScriptedEngine("e7e5", "d8h4"));
        finished.ChooseLevel(Level.Strongest);
        Assert.True(finished.TryPlayerMove(new Move(Squares.Index(5, 1), Squares.Index(5, 2)), out _));
        Assert.NotNull(await finished.PlayEngineMoveAsync());
        Assert.True(finished.TryPlayerMove(new Move(Squares.Index(6, 1), Squares.Index(6, 3)), out _));
        Assert.NotNull(await finished.PlayEngineMoveAsync());

        Assert.Equal(GameStage.Finished, finished.Stage);
        Assert.False(finished.ChangeLevel(Level.Weakest));
        Assert.Equal("Expert", finished.Level?.Name);
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