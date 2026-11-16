# BlzChess

Chess for casual players, in one browser tab. Open it, install it, play. No
account, no server, nothing stored where anyone else can see it.

It is a Blazor WebAssembly PWA. The rules live in C#, the opponent is Stockfish
compiled to WebAssembly and running in the same tab, and the site is static files
on GitHub Pages.

## What it is not

Not a chess platform. No ratings, no tournaments, no puzzles, no online play, no
accounts, no analysis library. If you want chess.com or lichess, use those.

## How a game goes

Pick an opponent, from "No idea how to play" to "Expert", and play. You are
White. Click a piece, click its destination, and the computer answers. The game
says when it is over: checkmate, stalemate, repetition, the fifty-move rule, or
insufficient material. "New game" takes you back to the level list.

The weakest level plays mostly random legal moves; the strongest plays Stockfish's
choice at depth 16. A refresh loses the game, because nothing is stored.

## Status

Playable against Stockfish at six levels. Every change shows you a spec before it
ships: see [`openspec/specs/`](openspec/specs) for what the product promises and
[`CHANGELOG.md`](CHANGELOG.md) for what it does today.

## Run it

Needs the .NET 10 SDK. The pinned version is in `global.json`.

```bash
dotnet run --project src/BlzChess.Web
```

The first run may print a URL such as `https://localhost:7231/`. Locally the app
serves at the server root. The published site sits under `/blz-chess/`, and the
deploy workflow rewrites the shell's base path for it, so nothing about the
sub-path leaks into the local loop.

For trimming at publish time, install the workload once:

```bash
dotnet workload install wasm-tools
```

## Build and test

```bash
dotnet restore --locked-mode
dotnet format --verify-no-changes --no-restore
dotnet build --no-restore -warnaserror
dotnet test --no-build
```

## Layout

```
src/BlzChess.Core/          chess rules, levels, and the game session. No browser.
src/BlzChess.Web/           Blazor WebAssembly standalone app (PWA)
src/BlzChess.Web/wwwroot/engine   vendored Stockfish wasm and its UCI shim
src/BlzChess.Web/wwwroot/pieces   the Cburnett piece set
tests/BlzChess.Core.Tests/  xUnit tests, perft included
.github/workflows/          ci.yml on every push, deploy.yml on main
```

## How work happens here

The process is in [`AGENTS.md`](AGENTS.md): every change is routed at a level, a
behavior change shows you a spec before it ships, and a contract change shows you
an ADR. Decisions are in [`adr/`](adr), conventions in
[`CONVENTIONS.md`](CONVENTIONS.md), and the shape of the system in
[`ARCHITECTURE.md`](ARCHITECTURE.md).

## Credits

The chess engine is [Stockfish](https://stockfishchess.org) compiled to WebAssembly
by [stockfish.js](https://github.com/nmrugg/stockfish.js), vendored unmodified at
`src/BlzChess.Web/wwwroot/engine` (build 19, lite network, single-threaded).

The pieces are the Cburnett set from [Lichess](https://github.com/lichess-org/lila)
(`wwwroot/pieces`), by Colin M.L. Burnett, CC BY-SA 3.0.

## Licence

GPLv3, because Stockfish ships inside the app and its licence travels with the
wasm. See [`LICENSE`](LICENSE).
