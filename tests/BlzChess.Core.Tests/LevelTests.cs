using BlzChess.Core;

namespace BlzChess.Core.Tests;

public class LevelTests
{
    [Fact]
    public void TheLadderRunsFromNobodyToExpertInOrder()
    {
        Assert.Equal(6, Level.All.Count);
        Assert.Equal(
            ["No idea how to play", "Just learning", "Beginner", "Casual", "Club player", "Expert"],
            Level.All.Select(level => level.Name));
        Assert.Equal("No idea how to play", Level.Weakest.Name);
        Assert.Equal("Expert", Level.Strongest.Name);
    }

    [Fact]
    public void AStrongerLevelIsNeverWeakerOnAnySetting()
    {
        for (var index = 1; index < Level.All.Count; index++)
        {
            var below = Level.All[index - 1];
            var above = Level.All[index];

            Assert.True(above.SkillLevel >= below.SkillLevel, $"{above.Name} searches less skilfully than {below.Name}");
            Assert.True(above.Depth >= below.Depth, $"{above.Name} searches shallower than {below.Name}");
            Assert.True(above.MoveTimeMs >= below.MoveTimeMs, $"{above.Name} thinks for less time than {below.Name}");
            Assert.True(above.RandomChance <= below.RandomChance, $"{above.Name} plays more randomly than {below.Name}");
        }
    }

    [Fact(DisplayName = "The weakest level often misses the best move")]
    public async Task TheWeakestLevelOftenMissesTheBestMove()
    {
        var bestMove = "e7e5";
        var misses = 0;

        for (var seed = 1; seed <= 5; seed++)
        {
            var session = new GameSession(Level.Weakest, new FakeEngine(bestMove), seed);
            Assert.True(session.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));

            var reply = await session.PlayEngineMoveAsync();

            if (reply is { } move && move.ToUci() != bestMove)
            {
                misses++;
            }
        }

        Assert.True(misses >= 2, $"the weakest level found the engine's move {5 - misses} times out of 5");
    }

    [Fact(DisplayName = "The strongest level plays the engine choice")]
    public async Task TheStrongestLevelPlaysTheEngineChoice()
    {
        var session = new GameSession(Level.Strongest, new FakeEngine("e7e5"), seed: 7);
        Assert.True(session.TryPlayerMove(new Move(Squares.Index(4, 1), Squares.Index(4, 3)), out _));

        var reply = await session.PlayEngineMoveAsync();

        Assert.Equal("e7e5", reply?.ToUci());
    }
}