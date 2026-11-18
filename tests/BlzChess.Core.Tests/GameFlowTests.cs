using BlzChess.Core;

namespace BlzChess.Core.Tests;

public class GameFlowTests
{
    [Fact(DisplayName = "The app starts a game on load")]
    public void TheAppStartsAGameOnLoad()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));

        Assert.Equal(GameStage.Playing, flow.Stage);
        Assert.Equal("No idea how to play", flow.Level.Name);
        Assert.Equal(Fen.StartingPosition, flow.Session.Game.Fen);
        Assert.Equal(Color.White, flow.Session.Game.Position.SideToMove);
        Assert.Empty(flow.Session.Game.Moves);
    }

    [Fact(DisplayName = "A new game starts again at the same level")]
    public async Task ANewGameStartsAgainAtTheSameLevel()
    {
        var flow = new GameFlow(new ScriptedEngine("e7e5", "d8h4"), Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(5, 1), Squares.Index(5, 2)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(6, 1), Squares.Index(6, 3)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.Equal(ResultKind.Checkmate, flow.Result.Kind);

        flow.NewGame();

        Assert.Equal(GameStage.Playing, flow.Stage);
        Assert.Equal("Expert", flow.Level.Name);
        Assert.Equal(Fen.StartingPosition, flow.Session.Game.Fen);
        Assert.Equal(Color.White, flow.Session.Game.Position.SideToMove);
        Assert.False(flow.Result.IsOver);
    }

    [Fact(DisplayName = "A refresh loses the game")]
    public async Task ARefreshLosesTheGame()
    {
        var first = new GameFlow(new FakeEngine("e7e5"), Level.All[3]);
        Assert.True(first.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await first.PlayEngineMoveAsync();
        Assert.NotEmpty(first.Session.Game.Moves);

        var afterRefresh = new GameFlow(new FakeEngine("e7e5"));

        Assert.Equal(GameStage.Playing, afterRefresh.Stage);
        Assert.Equal("No idea how to play", afterRefresh.Level.Name);
        Assert.Empty(afterRefresh.Session.Game.Moves);
        Assert.Equal(Fen.StartingPosition, afterRefresh.Session.Game.Fen);
    }

    [Fact(DisplayName = "Restarting starts the same level again")]
    public async Task RestartingStartsTheSameLevelAgain()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"), Level.All[3]);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await flow.PlayEngineMoveAsync();
        var played = flow.Session.Game.Moves.Count;

        var restarted = flow.Restart();

        Assert.True(restarted);
        Assert.Equal("Casual", flow.Level.Name);
        Assert.Equal(GameStage.Playing, flow.Stage);
        Assert.Equal(Fen.StartingPosition, flow.Session.Game.Fen);
        Assert.Equal(Color.White, flow.Session.Game.Position.SideToMove);
        Assert.Empty(flow.Session.Game.Moves);
        Assert.True(played > 0);
    }

    [Fact(DisplayName = "A restart records no result")]
    public async Task ARestartRecordsNoResult()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"), Level.All[3]);
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
        // Fool's mate, driven through the flow so its stage moves as the game does.
        var finished = new GameFlow(new ScriptedEngine("e7e5", "d8h4"), Level.Strongest);
        Assert.True(finished.TryPlayerMove(new Move(Squares.Index(5, 1), Squares.Index(5, 2)), out _));
        Assert.NotNull(await finished.PlayEngineMoveAsync());
        Assert.True(finished.TryPlayerMove(new Move(Squares.Index(6, 1), Squares.Index(6, 3)), out _));
        Assert.NotNull(await finished.PlayEngineMoveAsync());

        Assert.Equal(ResultKind.Checkmate, finished.Result.Kind);
        Assert.Equal(GameStage.Finished, finished.Stage);
        Assert.False(finished.Restart());
        Assert.Equal(GameStage.Finished, finished.Stage);
    }

    [Fact(DisplayName = "Changing the level keeps the position")]
    public async Task ChangingTheLevelKeepsThePosition()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"), Level.All[3]);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await flow.PlayEngineMoveAsync();
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(3, 1), Squares.Index(3, 3)), out _));

        var before = flow.Session.Game.Fen;
        var moves = flow.Session.Game.Moves.Count;
        var toMove = flow.Session.Game.Position.SideToMove;

        var changed = flow.ChangeLevel(Level.Strongest);

        Assert.True(changed);
        Assert.Equal("Expert", flow.Level.Name);
        Assert.Equal(before, flow.Session.Game.Fen);
        Assert.Equal(moves, flow.Session.Game.Moves.Count);
        Assert.Equal(toMove, flow.Session.Game.Position.SideToMove);
        Assert.Equal(GameStage.Playing, flow.Stage);
    }

    [Fact(DisplayName = "The new level answers")]
    public async Task TheNewLevelAnswers()
    {
        var engine = new FakeEngine("e7e5");
        // A level with no random share, so the engine is the one asked every time.
        var flow = new GameFlow(engine, Level.All[4]);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        await flow.PlayEngineMoveAsync();
        Assert.Equal("Club player", engine.LastLevel?.Name);

        flow.ChangeLevel(Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(3, 1), Squares.Index(3, 3)), out _));
        await flow.PlayEngineMoveAsync();

        Assert.Equal("Expert", engine.LastLevel?.Name);
    }

    [Fact]
    public async Task ChangingTheLevelAfterTheGameAppliesToTheNextOne()
    {
        var flow = new GameFlow(new ScriptedEngine("e7e5", "d8h4"), Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(5, 1), Squares.Index(5, 2)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(6, 1), Squares.Index(6, 3)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.Equal(GameStage.Finished, flow.Stage);

        Assert.True(flow.ChangeLevel(Level.Weakest));
        flow.NewGame();

        Assert.Equal(GameStage.Playing, flow.Stage);
        Assert.Equal("No idea how to play", flow.Level.Name);
        Assert.False(flow.Result.IsOver);
    }

    [Fact(DisplayName = "Taking back the move restores the position")]
    public async Task TakingBackTheMoveRestoresThePosition()
    {
        var flow = new GameFlow(new ScriptedEngine("e7e5"), Level.Strongest);
        var before = flow.Session.Game.Fen;
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.Equal(2, flow.Session.Game.Moves.Count);

        var undone = flow.UndoLastMove();

        Assert.True(undone);
        Assert.Equal(before, flow.Session.Game.Fen);
        Assert.Empty(flow.Session.Game.Moves);
        Assert.Equal(Color.White, flow.Session.Game.Position.SideToMove);
        Assert.False(flow.Result.IsOver);
        Assert.Equal(GameStage.Playing, flow.Stage);
    }

    [Fact(DisplayName = "There is nothing to take back at the start")]
    public void ThereIsNothingToTakeBackAtTheStart()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"));
        var before = flow.Session.Game.Fen;

        var undone = flow.UndoLastMove();

        Assert.False(undone);
        Assert.Equal(before, flow.Session.Game.Fen);
        Assert.Empty(flow.Session.Game.Moves);
    }

    [Fact(DisplayName = "The take-back is not offered once the game is over")]
    public async Task TheTakeBackIsNotOfferedOnceTheGameIsOver()
    {
        var flow = new GameFlow(new ScriptedEngine("e7e5", "d8h4"), Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(5, 1), Squares.Index(5, 2)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(6, 1), Squares.Index(6, 3)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.Equal(GameStage.Finished, flow.Stage);

        Assert.False(flow.UndoLastMove());
    }

    [Fact]
    public void TheTakeBackIsRefusedWhileItIsTheComputersTurn()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"), Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        var moves = flow.Session.Game.Moves.Count;

        // The engine has not replied yet, so the pair is half-played.
        var undone = flow.UndoLastMove();

        Assert.False(undone);
        Assert.Equal(moves, flow.Session.Game.Moves.Count);
        Assert.Equal(Color.Black, flow.Session.Game.Position.SideToMove);
    }

    [Fact]
    public async Task TheRepetitionStateSurvivesATakeBack()
    {
        // One round of knight shuffles: the start position comes back once.
        var flow = new GameFlow(
            new ScriptedEngine("b8c6", "c6b8", "b8c6", "c6b8", "b8c6", "c6b8"),
            Level.Strongest);
        var round = new[]
        {
            new Move(Squares.Index(6, 0), Squares.Index(5, 2)),
            new Move(Squares.Index(5, 2), Squares.Index(6, 0)),
        };

        foreach (var player in round)
        {
            Assert.True(flow.TryPlayerMove(player, out _));
            Assert.NotNull(await flow.PlayEngineMoveAsync());
        }

        Assert.Equal(4, flow.Session.Game.Moves.Count);
        Assert.False(flow.Result.IsOver);

        // Take the round back entirely.
        Assert.True(flow.UndoLastMove());
        Assert.True(flow.UndoLastMove());
        Assert.Empty(flow.Session.Game.Moves);
        Assert.Equal(Fen.StartingPosition, flow.Session.Game.Fen);
        Assert.False(flow.Result.IsOver);

        // The same two rounds again: if the rebuilt repetition keys were stale, the
        // draw would come early; if they were missing, it would never come.
        foreach (var roundIndex in (int[])[0, 1])
        {
            foreach (var player in round)
            {
                Assert.True(flow.TryPlayerMove(player, out _));
                Assert.NotNull(await flow.PlayEngineMoveAsync());
            }
        }

        Assert.Equal(8, flow.Session.Game.Moves.Count);
        Assert.Equal(ResultKind.ThreefoldRepetition, flow.Result.Kind);
    }

    [Fact]
    public async Task TheEngineMovesAfterThePlayerAndNotBefore()
    {
        var engine = new FakeEngine("e7e5");
        var flow = new GameFlow(engine, Level.Strongest);

        Assert.Null(await flow.PlayEngineMoveAsync());
        Assert.Equal(0, engine.Calls);

        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.Equal(1, engine.Calls);
        Assert.Equal(Color.White, flow.Session.Game.Position.SideToMove);
    }

    [Fact]
    public void ThePlayerCannotMoveWhileItIsTheComputersTurn()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"), Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));

        var blackMove = new Move(Squares.Index(4, 6), Squares.Index(4, 4));
        var refused = flow.TryPlayerMove(blackMove, out _);

        Assert.False(refused);
        Assert.Equal(Color.Black, flow.Session.Game.Position.SideToMove);
    }

    [Fact]
    public async Task ACheckmatedGameFinishes()
    {
        var flow = new GameFlow(new FakeEngine("e7e5"), Level.Strongest);

        foreach (var uci in new[] { "f2f3", "e7e5", "g2g4", "d8h4" })
        {
            if (flow.Session.Game.Position.SideToMove == Color.White)
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

        Assert.Equal(ResultKind.Checkmate, flow.Session.Result.Kind);
    }
}