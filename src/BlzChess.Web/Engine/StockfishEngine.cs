using BlzChess.Core;

using Microsoft.JSInterop;

namespace BlzChess.Web.Engine;

/// <summary>
/// Stockfish, reached through <c>wwwroot/engine/engine.js</c>. The module is
/// imported on first use, after the board is already interactive, and the wasm it
/// starts lives in the same folder.
/// </summary>
public sealed class StockfishEngine : IEngine, IAsyncDisposable
{
    /// <summary>The vendored build, and the hash of the file that build loads.</summary>
    public const string AssetPath = "engine/stockfish-19-lite-single.wasm";

    public const string AssetSha256 = "ee4f115bdb2d32cadf96cada104a05b5ec257d7ad6f57582190b49bc71f096de";

    private readonly IJSRuntime _js;
    private IJSObjectReference? _module;

    public StockfishEngine(IJSRuntime js)
    {
        _js = js ?? throw new ArgumentNullException(nameof(js));
    }

    public async Task<Move?> BestMoveAsync(string fen, Level level, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(level);
        if (string.IsNullOrWhiteSpace(fen))
        {
            return null;
        }

        var module = await ModuleAsync(cancellationToken).ConfigureAwait(false);
        var uci = await module
            .InvokeAsync<string?>("bestMove", cancellationToken, fen, level.SkillLevel, level.Depth, level.MoveTimeMs)
            .ConfigureAwait(false);

        return uci is not null && Move.TryParseUci(uci, out var move) ? move : null;
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is null)
        {
            return;
        }

        var module = _module;
        _module = null;
        try
        {
            await module.InvokeVoidAsync("quit").ConfigureAwait(false);
            await module.DisposeAsync().ConfigureAwait(false);
        }
        catch (JSDisconnectedException)
        {
            // The tab navigated away; there is nothing left to clean up.
        }
    }

    private async Task<IJSObjectReference> ModuleAsync(CancellationToken cancellationToken)
    {
        if (_module is not null)
        {
            return _module;
        }

        _module = await _js.InvokeAsync<IJSObjectReference>("import", cancellationToken, "./engine/engine.js")
            .ConfigureAwait(false);
        return _module;
    }
}