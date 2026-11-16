# Architecture

Last verified: 2026-11-16, at the change that made the game playable.

BlzChess is a static chess app: the browser downloads it once, caches it, and
plays without asking anyone else for anything. Why that shape was chosen is in
[`adr/0001-blazor-wasm-on-github-pages.md`](adr/0001-blazor-wasm-on-github-pages.md).

## Context

```mermaid
flowchart LR
  player(["Person: casual chess player"])

  system["Software System: BlzChess"]

  gh["External: GitHub Pages<br/>static hosting"]

  player -->|"Loads the site over HTTPS"| gh
  gh -->|"Serves the app and its assets"| player

  classDef person fill:#08427B,color:#fff,stroke:#052E56
  classDef system fill:#1168BD,color:#fff,stroke:#0B4884
  classDef external fill:#999999,color:#fff,stroke:#666666,stroke-dasharray:5 5

  class player person
  class system system
  class gh external
```

## Containers

```mermaid
flowchart LR
  subgraph browser["Browser tab"]
    ui["Container: Blazor WebAssembly app<br/>.NET 10, PWA"]
    rules["Container: BlzChess.Core<br/>chess rules library"]
    engine["Container: Stockfish wasm<br/>Web Worker, UCI over postMessage"]
    storage[("Store: service worker cache<br/>app assets, engine build")]
  end

  player(["Person: casual chess player"])

  pages["External: GitHub Pages"]
  actions["External: GitHub Actions"]

  player -->|"Clicks and moves, HTTPS"| ui
  ui -->|"Asserts legality, C# calls"| rules
  ui -->|"Asks for the best move, UCI"| engine
  engine -->|"Best move, UCI"| ui
  ui -->|"Caches assets, HTTP fetch"| storage
  actions -->|"Publishes static files, HTTPS"| pages
  pages -->|"Serves the app, HTTPS"| ui

  classDef person fill:#08427B,color:#fff,stroke:#052E56
  classDef container fill:#438DD5,color:#fff,stroke:#2E6295
  classDef store fill:#438DD5,color:#fff,stroke:#2E6295
  classDef external fill:#999999,color:#fff,stroke:#666666,stroke-dasharray:5 5

  class player person
  class ui,rules,engine container
  class storage store
  class pages,actions external
```

## Legend

| Shape | C4 element |
|---|---|
| `(["..."])` | Person |
| `["..."]` | Software system, container, or external system |
| `[("...")]` | Data store |
| dashed border | External system, outside this repository's control |

## Containers

| Container | Responsibility | Path | Technology |
|---|---|---|---|
| Blazor WebAssembly app | the UI, routing, install, and the game session | `src/BlzChess.Web` | Blazor WebAssembly standalone, .NET 10, PWA service worker |
| BlzChess.Core | board state and chess rules; no UI, no browser | `src/BlzChess.Core` | C# class library, `net10.0` |
| Stockfish wasm | chooses the opponent's move | `src/BlzChess.Web/wwwroot/engine` | Stockfish 19 compiled to WebAssembly, lite network, single-threaded, in a Web Worker |
| Service worker cache | makes a repeat visit and offline play work | `src/BlzChess.Web/wwwroot/service-worker.published.js` | browser Cache Storage |
| CI | builds and tests every push | `.github/workflows/ci.yml` | GitHub Actions, `ubuntu-latest` |
| Deploy | publishes the site from `main` | `.github/workflows/deploy.yml` | GitHub Actions Pages deployment |

Hosting is GitHub Pages: static files only, no server, no database, no queue. The
engine is fetched on the first computer move and cached by the service worker, so
the page is interactive before the engine arrives.

## Open questions

- Whether the lite network plays weakly enough for the bottom levels and strongly
  enough for the top one.
- Whether threaded Stockfish is worth leaving GitHub Pages for. It needs COOP/COEP
  headers, which Pages cannot send.

## Links

- Decision: [`adr/0001-blazor-wasm-on-github-pages.md`](adr/0001-blazor-wasm-on-github-pages.md)
- Behavior: [`openspec/specs/`](openspec/specs) (empty until the first feature change)
- Rules for code: [`CONVENTIONS.md`](CONVENTIONS.md)
