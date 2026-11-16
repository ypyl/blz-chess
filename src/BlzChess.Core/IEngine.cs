namespace BlzChess.Core;

/// <summary>
/// The computer's side. One method, so the rules and the session can be tested
/// without a browser or a wasm binary: the real implementation talks to Stockfish
/// over UCI, and a test uses a fake that answers what the test needs.
/// </summary>
public interface IEngine
{
    /// <summary>
    /// The engine's preferred move in <paramref name="fen"/> at the given level, or
    /// null when the engine could not answer. The caller falls back to any legal move.
    /// </summary>
    Task<Move?> BestMoveAsync(string fen, Level level, CancellationToken cancellationToken = default);
}