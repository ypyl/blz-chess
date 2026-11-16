# 0001. Blazor WebAssembly and Stockfish, statically hosted on GitHub Pages

- Status: Accepted
- Date: 2026-11-16

## Context

The product is a chess web app for casual players: a single-page app, installable
as a PWA, with no server, no accounts, and no stored user data. It is a pet
project with no load target, so the cost that matters is the maintainer's time,
not infrastructure.

A static site has no server to run an engine on, so the opponent has to be code
the browser runs. The maintainer writes C# and .NET daily, .NET 10 is the current
LTS, and the account the project already uses is GitHub.

## Decision

We will build one C# codebase: a chess rules library with no UI or browser
dependency, plus a Blazor WebAssembly standalone app that references it and
installs as a PWA.

The opponent is Stockfish compiled to WebAssembly (the `stockfish` npm build,
version pinned and vendored under `wwwroot/`), loaded as a Web Worker from the
site's own origin and driven over UCI by a thin JS shim behind `IJSRuntime`. The
single-threaded build ships first, because threaded Stockfish needs
`SharedArrayBuffer` and therefore COOP/COEP headers that GitHub Pages cannot
send.

The site ships as static files to GitHub Pages through a GitHub Actions workflow.
Nothing runs on a server.

## Consequences

- The rules library is testable in plain xUnit, with no browser. It is still
  needed even with Stockfish: the UI cannot round-trip every legal move, check,
  or mate call through UCI and stay responsive.
- Stockfish is GPLv3, and shipping its wasm passes that licence on. This
  repository becomes GPLv3 and keeps a `LICENSE` file; the source is already
  public, so the obligation costs nothing here. Accepted for a pet project, and
  forbidden: shipping Stockfish in a repository that is not GPLv3-compatible.
- Two builds, one tradeoff. The full network is 94.6 MB and the lite network is
  1.8 MB. The service worker caches either after the first load, but first
  impression decides: lite ships first, and the full net is only adopted with
  evidence that lite plays too weakly.
- Single-threaded only, so strength per second of thinking is lower. Acceptable
  for casual play. Moving to threaded Stockfish is an L2 hosting change, not a
  build flag: it is what Cloudflare Pages or Azure Static Web Apps buy, at the
  cost of a second account and a second provider.
- GitHub Pages serves static files only: no custom headers, no rewrite rules, no
  server. Client-side routes need `index.html` copied to `404.html`, and the site
  publishes under a sub-path, so `base href` is `/blz-chess/`.
- A 1.8 MB engine is real weight in a PWA. It loads after the UI is interactive
  and only when a game starts, so the app is usable before the engine arrives.
- Deploy is a workflow, not a command. A broken workflow blocks a release until
  it is fixed in a change of its own.
- Forbidden without a new ADR: a server, a database, accounts, or online
  multiplayer. Each one breaks the stateless premise the product is built on.

## Alternatives

- Stockfish loaded from a third-party CDN at runtime: skips vendoring and the
  licence coupling, but breaks offline play and makes someone else's host load
  the app's core asset.
- A C# engine (`LynxChess.Lynx`) compiled to the same WebAssembly target: one
  language and no GPLv3, but an unproven WASM path and weaker play.
- React 19 + TypeScript + Vite: the larger chess ecosystem and the smallest
  payload, but a second language for a maintainer who lives in C#.
- Svelte 5 + TypeScript + Vite: less code than React and a smaller bundle, same
  language cost, thinner ecosystem.
- Azure Static Web Apps instead of GitHub Pages: native .NET pairing and headers
  for threaded Stockfish, plus an Azure subscription and a second account.
- Cloudflare Pages instead of GitHub Pages: headers, compression, and preview
  deploys, so threaded Stockfish works, but a second account and provider.
- Azure Static Web Apps or Cloudflare Pages now, for threads from day one: the
  casual opponent does not need them, and a second account is a cost paid before
  it buys anything.
