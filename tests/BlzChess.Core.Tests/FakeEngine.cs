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

    public Task<Move?> BestMoveAsync(string fen, Level level, CancellationToken cancellationToken = default)
    {
        Calls++;
        var move = _uci is not null && Move.TryParseUci(_uci, out var parsed) ? parsed : (Move?)null;
        return Task.FromResult(move);
    }
}