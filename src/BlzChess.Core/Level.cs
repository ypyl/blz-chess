namespace BlzChess.Core;

/// <summary>
/// One opponent level: what the engine is asked for, and how often it is bypassed
/// for a random legal move. The weakest level exists because Stockfish's floor is
/// far above "does not know the rules".
/// </summary>
public sealed record Level(int Index, string Name, int SkillLevel, int Depth, int MoveTimeMs, double RandomChance)
{
    /// <summary>The ladder, weakest first. Order is the page's order and the spec's order.</summary>
    public static IReadOnlyList<Level> All { get; } =
    [
        new(1, "No idea how to play", SkillLevel: 0, Depth: 1, MoveTimeMs: 100, RandomChance: 0.80),
        new(2, "Just learning", SkillLevel: 0, Depth: 2, MoveTimeMs: 150, RandomChance: 0.50),
        new(3, "Beginner", SkillLevel: 1, Depth: 4, MoveTimeMs: 200, RandomChance: 0.25),
        new(4, "Casual", SkillLevel: 5, Depth: 6, MoveTimeMs: 400, RandomChance: 0.05),
        new(5, "Club player", SkillLevel: 12, Depth: 10, MoveTimeMs: 800, RandomChance: 0.0),
        new(6, "Expert", SkillLevel: 20, Depth: 16, MoveTimeMs: 1500, RandomChance: 0.0),
    ];

    public static Level Weakest => All[0];

    public static Level Strongest => All[^1];

    public static Level ByIndex(int index) =>
        All.FirstOrDefault(level => level.Index == index)
        ?? throw new ArgumentOutOfRangeException(nameof(index), index, "No level with that index");
}