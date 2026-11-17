using BlzChess.Core;

namespace BlzChess.Core.Tests;

/// <summary>An engine that answers one fixed move, so tests never need a browser.</summary>
internal sealed class FakeEngine : IEngine
{
    private readonly string? _uci;

    public FakeEngine(string? uci)
    {
        _uci = uci;
    }

    public int Calls { get; private set; }

    public Level? LastLevel { get; private set; }

    public Task<Move?> BestMoveAsync(string fen, Level level, CancellationToken cancellationToken = default)
    {
        Calls++;
        LastLevel = level;
        var move = _uci is not null && Move.TryParseUci(_uci, out var parsed) ? parsed : (Move?)null;
        return Task.FromResult(move);
    }
}
/// <summary>An engine that answers a queued script of moves, so a game can be driven to an end.</summary>
internal sealed class ScriptedEngine : IEngine
{
    private readonly Queue<string> _moves;

    public ScriptedEngine(params string[] moves)
    {
        _moves = new Queue<string>(moves);
    }

    public Task<Move?> BestMoveAsync(string fen, Level level, CancellationToken cancellationToken = default)
    {
        var move = _moves.Count > 0 && Move.TryParseUci(_moves.Dequeue(), out var parsed) ? parsed : (Move?)null;
        return Task.FromResult(move);
    }
}