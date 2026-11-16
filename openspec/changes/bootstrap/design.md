# Design: how the stack lands in this repository

The change-local approach. The decision itself is in
[`adr/0001-blazor-wasm-on-github-pages.md`](../../../adr/0001-blazor-wasm-on-github-pages.md);
this file says how it is built here.

## Layout

```
BlzChess.sln
src/BlzChess.Core/          chess rules, no UI, no browser, no I/O
src/BlzChess.Web/           Blazor WebAssembly standalone app, PWA
tests/BlzChess.Core.Tests/  xUnit, one project per library under test
```

The dependency arrow points one way: `Web` references `Core`, never the reverse.
`Core` targets `net10.0` with `Nullable` and `TreatWarningsAsErrors` on, so a
warning cannot reach trunk from either project.

There is deliberately no test project for the UI yet. Rules are the part that can
be wrong without looking wrong; component behavior gets bUnit the first time a
change makes a component worth testing.

## Base path and client-side routes

GitHub Pages serves the site from `https://<owner>.github.io/blz-chess/`, so
`wwwroot/index.html` carries `<base href="/blz-chess/" />`. It is written into
the template once and checked by a test-free grep, because a wrong base path
produces a blank page with a green build.

Pages answers an unknown path with `404.html`. The build copies `index.html` to
`404.html` after publish, which is what lets a deep link such as
`/blz-chess/game` reach the router instead of GitHub's error page.

## Publish and compression

`dotnet publish src/BlzChess.Web -c Release` is the whole static site; the
artifact is `publish/wwwroot`. Pages does not negotiate per-file precompressed
assets, so the publish disables the framework's precompressed output and ships
uncompressed files that Pages compresses at the edge. `.nojekyll` goes in
`wwwroot` so Pages does not try to process the site as Jekyll.

Trimming stays on, AOT stays off: AOT multiplies publish time and payload for a
game whose first move is not latency-sensitive. Turning it on is a later L2 with
evidence, not a guess.

## Toolchain

`global.json` pins the SDK to the 10.0 band with `rollForward: latestFeature`, so
local and CI use the same major. NuGet restore runs with
`RestorePackagesWithLockFile=true` and commits `packages.lock.json`; CI restores
in locked mode, so a transitive upgrade is a diff a human can see.

## Two workflows, two jobs

`.github/workflows/ci.yml` runs on every push and pull request: `dotnet restore
--locked-mode`, `dotnet format --verify-no-changes`, `dotnet build
-warnaserror`, `dotnet test`. It needs no secrets and no permissions beyond
`contents: read`.

`.github/workflows/deploy.yml` runs on a push to `main`, gated on CI passing,
publishes, copies `index.html` to `404.html`, and uploads the Pages artifact. It
holds `pages: write` and `id-token: write`, and those permissions exist on that
workflow only. Both workflows pin action versions to a major tag and take no
input from a pull request, so a fork cannot reach the deploy job.

## The engine

This change decides the engine; the feature change that adds an opponent wires
it. The shape it will take:

- `wwwroot/engine/stockfish-lite-single.{js,wasm}`, vendored from the pinned
  `stockfish` package and served from the site's own origin. No CDN at runtime,
  so offline play works and nobody else's host stands between the user and the
  asset.
- `wwwroot/engine/engine.js`, a ~50-line shim that owns the Worker and speaks
  UCI in both directions (`uci`, `isready`, `position`, `go`, `bestmove`), with
  a promise-based request id so a Blazor call can await one line.
- A C# wrapper in `Web` over `IJSRuntime` that exposes "start", "set position",
  "find best move", with no UCI vocabulary leaking past it.
- The engine loads lazily, after the UI is interactive and only when a game
  starts, and the service worker caches it. First paint never waits for 1.8 MB.
- The version is a constant in one place, and the vendored file's sha256 is
  recorded next to it so an upgrade is a visible diff rather than a silent swap.

Single-threaded only: `stockfish-19.wasm` (94.6 MB, full network) and the
threaded variant are deliberately out. The threaded build needs
`SharedArrayBuffer`, which needs COOP/COEP, which GitHub Pages cannot send.

## Licence

Stockfish is GPLv3, so the repository ships a `LICENSE` with GPLv3 and the README
says so. The source is public, which is the whole obligation. A change that
vendors a package carrying a different licence is L2 for that reason alone.

## What the local loop is

`dotnet run --project src/BlzChess.Web` for the app, `dotnet test` for the rules,
`bash scripts/verify.sh` before a commit. Project facts name all three, and
`scripts/verify.sh` is what the agent reports at the end of a change.

## Verification of this change

`dotnet build` succeeds with warnings as errors, `dotnet test` runs the rules
tests (a placeholder rule test pairing the first scenario), the two workflows
pass `actionlint`-clean review by reading, and the app renders its shell at the
base path in `dotnet run`.
