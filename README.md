# BlzChess

Chess for casual players, in one browser tab. Open it, install it, play. No
account, no server, nothing stored where anyone else can see it.

It is a Blazor WebAssembly PWA. The rules live in C#, the opponent is Stockfish
compiled to WebAssembly and running in the same tab, and the site is static files
on GitHub Pages.

## What it is not

Not a chess platform. No ratings, no tournaments, no puzzles, no online play, no
accounts, no analysis library. If you want chess.com or lichess, use those.

## Status

The shell: the app builds, installs, and serves an empty board page. Chess
arrives one change at a time, each with a spec you can read before it ships. See
[`openspec/specs/`](openspec/specs) for what the product promises and
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
src/BlzChess.Core/          chess rules. No UI, no browser, no I/O.
src/BlzChess.Web/           Blazor WebAssembly standalone app (PWA)
tests/BlzChess.Core.Tests/  xUnit tests for the rules
.github/workflows/          ci.yml on every push, deploy.yml on main
```

## How work happens here

The process is in [`AGENTS.md`](AGENTS.md): every change is routed at a level, a
behavior change shows you a spec before it ships, and a contract change shows you
an ADR. Decisions are in [`adr/`](adr), conventions in
[`CONVENTIONS.md`](CONVENTIONS.md), and the shape of the system in
[`ARCHITECTURE.md`](ARCHITECTURE.md).

## Licence

GPLv3, because Stockfish ships inside the app and its licence travels with the
wasm. See [`LICENSE`](LICENSE).
