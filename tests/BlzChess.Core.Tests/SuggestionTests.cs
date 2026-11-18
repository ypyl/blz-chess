using BlzChess.Core;

namespace BlzChess.Core.Tests;

/// <summary>
/// The switch that shows a suggested move. What matters here is what the page asks for,
/// what it is allowed to keep, and that a suggestion never becomes a move.
/// </summary>
public class SuggestionTests
{
    private static int Sq(string name)
    {
        Assert.True(Squares.TryParse(name, out var square));
        return square;
    }

    [Fact(DisplayName = "No suggested move is shown until the switch is on")]
    public void NoSuggestedMoveIsShownUntilTheSwitchIsOn()
    {
        var engine = new FakeEngine("g1f3");
        var flow = new GameFlow(engine);

        Assert.False(flow.Hints);
        Assert.Null(flow.Suggestion);
        Assert.Equal(0, engine.Calls);
    }

    [Fact(DisplayName = "The suggested move is asked for at full strength")]
    public async Task TheSuggestedMoveIsAskedForAtFullStrength()
    {
        // The opponent is the weakest level, whose own moves are mostly random. A hint
        // asked at that strength would recommend blunders.
        var engine = new FakeEngine("g1f3");
        var flow = new GameFlow(engine, Level.Weakest);

        await flow.ToggleHintsAsync();

        Assert.True(flow.Hints);
        Assert.Equal(1, engine.Calls);
        Assert.NotNull(engine.LastLevel);
        Assert.Equal(20, engine.LastLevel!.SkillLevel);
        Assert.Equal(Level.Coach, engine.LastLevel);
        Assert.NotEqual(flow.Level.SkillLevel, engine.LastLevel.SkillLevel);
    }

    [Fact(DisplayName = "The suggested move is one the player may play")]
    public async Task TheSuggestedMoveIsOneThePlayerMayPlay()
    {
        var flow = new GameFlow(new FakeEngine("g1f3"));

        await flow.ToggleHintsAsync();

        var suggestion = Assert.IsType<Move>(flow.Suggestion);
        Assert.Contains(suggestion, flow.Session.Game.LegalMoves());
        Assert.Equal(Sq("g1"), suggestion.From);
        Assert.Equal(Sq("f3"), suggestion.To);
    }

    [Fact(DisplayName = "A suggestion the rules do not allow is not shown")]
    public async Task ASuggestionTheRulesDoNotAllowIsNotShown()
    {
        // An engine that answers with something illegal, which the page must never draw.
        var flow = new GameFlow(new FakeEngine("e2e5"));

        await flow.ToggleHintsAsync();

        Assert.Null(flow.Suggestion);
    }

    [Fact(DisplayName = "Switching hints off hides the suggested move")]
    public async Task SwitchingHintsOffHidesTheSuggestedMove()
    {
        var flow = new GameFlow(new FakeEngine("g1f3"));
        await flow.ToggleHintsAsync();
        Assert.NotNull(flow.Suggestion);

        await flow.ToggleHintsAsync();

        Assert.False(flow.Hints);
        Assert.Null(flow.Suggestion);
    }

    [Fact(DisplayName = "A suggested move changes nothing in the game")]
    public async Task ASuggestedMoveChangesNothingInTheGame()
    {
        var engine = new FakeEngine("g1f3");
        var flow = new GameFlow(engine, Level.All[3]);
        var fen = flow.Session.Game.Fen;
        var level = flow.Level;

        await flow.ToggleHintsAsync();

        Assert.NotNull(flow.Suggestion);
        Assert.Equal(fen, flow.Session.Game.Fen);
        Assert.Empty(flow.Session.Game.Moves);
        Assert.Equal(level, flow.Level);
        Assert.Equal(Color.White, flow.Session.Game.Position.SideToMove);
        Assert.False(flow.Result.IsOver);

        // Ignoring it changes nothing either: the player plays their own move.
        Assert.True(flow.TryPlayerMove(new Move(Sq("e2"), Sq("e4")), out _));
        Assert.Equal("e2e4", flow.Session.Game.Moves[^1].ToUci());
    }

    [Fact(DisplayName = "The suggestion follows the position after the computer answers")]
    public async Task TheSuggestionFollowsThePositionAfterTheComputerAnswers()
    {
        var engine = new CoachEngine(new ScriptedEngine("e7e5"), "g1f3", "b1c3");
        var flow = new GameFlow(engine, Level.Strongest);

        await flow.ToggleHintsAsync();
        var first = Assert.IsType<Move>(flow.Suggestion);
        Assert.Contains(first, flow.Session.Game.LegalMoves());

        Assert.True(flow.TryPlayerMove(new Move(Sq("e2"), Sq("e4")), out _));
        Assert.Null(flow.Suggestion);

        Assert.NotNull(await flow.PlayEngineMoveAsync());

        var second = Assert.IsType<Move>(flow.Suggestion);
        Assert.NotEqual(first, second);
        Assert.Contains(second, flow.Session.Game.LegalMoves());
        Assert.Equal(Color.White, flow.Session.Game.Position.SideToMove);
    }

    [Fact(DisplayName = "No suggestion is offered once the game is over")]
    public async Task NoSuggestionIsOfferedOnceTheGameIsOver()
    {
        // Fool's mate: 1. f3 e5 2. g4 Qh4#.
        var engine = new ScriptedEngine("e7e5", "d8h4");
        var flow = new GameFlow(engine, Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Sq("f2"), Sq("f3")), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.True(flow.TryPlayerMove(new Move(Sq("g2"), Sq("g4")), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());
        Assert.True(flow.Result.IsOver);

        await flow.ToggleHintsAsync();

        Assert.Null(flow.Suggestion);
    }

    [Fact(DisplayName = "A take-back drops the suggestion for the old position")]
    public async Task ATakeBackDropsTheSuggestionForTheOldPosition()
    {
        var engine = new CoachEngine(new ScriptedEngine("e7e5"), "g1f3", "g1f3");
        var flow = new GameFlow(engine, Level.Strongest);
        Assert.True(flow.TryPlayerMove(new Move(Sq("e2"), Sq("e4")), out _));
        Assert.NotNull(await flow.PlayEngineMoveAsync());

        await flow.ToggleHintsAsync();
        Assert.NotNull(flow.Suggestion);

        Assert.True(flow.UndoLastMove());
        Assert.Null(flow.Suggestion);

        // The page asks again once the board is back to a position it is the player's turn
        // in, which is what the requirement says the arrow belongs to.
        await flow.SuggestAsync();
        Assert.NotNull(flow.Suggestion);
        Assert.True(flow.Hints);
    }

    [Fact(DisplayName = "A new game drops the suggestion and keeps the switch")]
    public async Task ANewGameDropsTheSuggestionAndKeepsTheSwitch()
    {
        var flow = new GameFlow(new CoachEngine(new ScriptedEngine("e7e5"), "g1f3", "g1f3"), Level.Strongest);
        await flow.ToggleHintsAsync();
        Assert.NotNull(flow.Suggestion);

        flow.NewGame();
        Assert.Null(flow.Suggestion);

        await flow.SuggestAsync();
        Assert.NotNull(flow.Suggestion);
        Assert.True(flow.Hints);
        Assert.Equal(Fen.StartingPosition, flow.Session.Game.Fen);
    }

    [Fact(DisplayName = "The coach is not a level the page offers")]
    public void TheCoachIsNotALevelThePageOffers()
    {
        Assert.DoesNotContain(Level.All, level => level.Name == "Coach");
        Assert.Equal(20, Level.Coach.SkillLevel);
        Assert.Equal(0.0, Level.Coach.RandomChance);
        Assert.All(Level.All, level => Assert.NotEqual(0, level.Index));
    }
}

/// <summary>
/// Two engines in one: the coach answers questions at its own strength, the opponent
/// plays its script. A single script could not tell a hint from a reply, which is exactly
/// the confusion this feature has to avoid.
/// </summary>
internal sealed class CoachEngine : IEngine
{
    private readonly IEngine _opponent;
    private readonly Queue<string> _suggestions;

    public CoachEngine(IEngine opponent, params string[] suggestions)
    {
        _opponent = opponent;
        _suggestions = new Queue<string>(suggestions);
    }

    public int CoachCalls { get; private set; }

    public Task<Move?> BestMoveAsync(string fen, Level level, CancellationToken cancellationToken = default)
    {
        if (level != Level.Coach)
        {
            return _opponent.BestMoveAsync(fen, level, cancellationToken);
        }

        CoachCalls++;
        var move = _suggestions.Count > 0 && Move.TryParseUci(_suggestions.Dequeue(), out var parsed)
            ? parsed
            : (Move?)null;
        return Task.FromResult(move);
    }
}