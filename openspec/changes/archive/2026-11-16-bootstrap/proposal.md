# Bootstrap: make this repository the chess app

L2

## Why

The repository is still a template. `AGENTS.md` carries the placeholder
paragraph, Project facts are empty, and `adr/` holds no decision. Nothing here
can be built, run, or verified, because there is no stack, no toolchain, and no
command to run.

## What Changes

The repository becomes a stateless chess web app for casual players: one Blazor
WebAssembly PWA, a C# chess rules library with no browser dependency, Stockfish
compiled to WebAssembly as the opponent, static hosting on GitHub Pages, and a
GitHub Actions pipeline. The change fixes the stack and the scaffold and fills
the contract documents. It promises no chess behavior: the first feature change
defines that.

## Routing

0. **Observable behavior changes?** No. No spec exists, and this change states no
   chess requirement. It fixes the stack and the scaffold.
1. **Undoable in one commit?** No. Reversing the stack rewrites every later
   change, and the deploy path is a published URL.
2. **A machine can prove it correct?** For the scaffold, yes: build, tests, and
   the CI run. For the stack choice itself, no. It is a judgement, which is why
   the gate is the ADR.
3. **Does anything outside depend on it?** Yes.
   [`adr/0001-blazor-wasm-on-github-pages.md`](../../../adr/0001-blazor-wasm-on-github-pages.md)
   holds the decision, and every later change inherits the toolchain, the trunk
   branch, and the deploy path from it.

Level **L2**, `skip_specs: true`: no product behavior to specify, one gate over
the ADR, this summary, the design, and the plan.

Revised at r2 after the reviewer asked whether Stockfish can run in the browser.
It can, and it now carries the opponent role in the ADR, with the licence and the
hosting constraints it brings.

## Answers

The human's words, unedited.

| # | Question | Answer |
|---|---|---|
| 1 | What does it do? | "Chess game, web app spa (pwa) allowing to play chess, stateless." |
| 2 | Who is it for? | "Casualy chess players" |
| 3 | What is it not? | "Complex chess platform like chess.com or lichess" |
| 4 | What data, and where? | "No data, completely stateless" |
| 5 | Load and users? | "No users, as it is pet project" |
| 6 | Platform? | web SPA + PWA, from answer 1. Offline behavior was not stated; assumed in A1. |
| 7 | Stack preference? | "Blazor wasm" |
| 8 | Where does it run? | "GIthub pages, static page" |
| 9 | CI? | "GitHub CI/CD" |
| 10 | Secrets? | "gh tool is available". No secret named; assumed in A2. |

## Assumptions

Every answer the human did not give, with the trigger that reopens it.

- **A1: installable offline PWA.** Answer 1 says PWA, so the app ships a service
  worker, installs to a home screen, and plays offline after the first load. In
  progress state stays in browser storage on the device, never in the repository
  and never on a server. Revisit when a change needs online play: that answer and
  "stateless" both fail together and need a new ADR.
- **A2: no secrets at all.** Answer 10 named a tool, not a secret. The app carries
  no keys, no `.env` file, and no `.env.example`. The deploy workflow uses only
  the repository's `GITHUB_TOKEN` and an OIDC `id-token` scoped to the deploy job.
  Revisit when a change adds the first credential: it gets a Secrets line before
  it gets a value.
- **A3: the site publishes at `/blz-chess/`.** The repository name is known;
  the remote does not exist yet. Revisit if the repo becomes `<owner>.github.io`
  or gets a custom domain: the base path becomes `/`.
- **A4: GitHub Pages limits are accepted.** No custom headers, no server-side
  rewrite, no preview deploys. Mitigated with `base href`, a `404.html` copy of
  `index.html`, and `.nojekyll`. Revisit if the app needs rewrite control, header
  control, or per-PR previews: that is an L2 hosting change, not a workaround.
- **A5: the rules library is written here, not adopted.** `.NET` chess libraries
  exist (`Gera.Chess`, `ChessDotNetCore`, `Rudzoft.ChessLib`), but all are small
  and thinly maintained. The rules library stays ours for now; the ADR's
  alternatives record the adoption path. Revisit at the existing L2 rule: adding
  one of them later replaces core code, so it needs an ADR of its own.
- **A6: this change ships a shell, not a game.** Whether the first feature is
  hot-seat or against Stockfish is undecided and out of scope here. Revisit at
  the first L1.
- **A7: stock Blazor layout.** The visual language is a later change; no
  `DESIGN.md` this time. Revisit at the first change that touches look and feel.
- **A8: the lite Stockfish network ships first.** 1.8 MB against 94.6 MB, cached
  by the service worker either way. Revisit when a game against lite feels too
  weak or too slow: the full network is a one-line swap plus a download budget
  the human approves.
- **A9: single-threaded Stockfish is enough.** Threads need COOP/COEP headers,
  which GitHub Pages cannot send. Revisit if response time or strength disappoints:
  the fix is an L2 hosting change to Cloudflare Pages or Azure Static Web Apps,
  not a build flag.
- **A10: the repository becomes GPLv3.** Stockfish ships inside the app, and its
  licence travels with the wasm. Revisit only if the engine is replaced by a
  permissively licensed one, and never by shipping GPL code in a closed repo.

## Capabilities

None. `skip_specs: true`, and the first feature change creates `gameplay`.

## Impact

Creates `src/BlzChess.Core`, `src/BlzChess.Web`, `tests/BlzChess.Core.Tests`, the
solution and build files, `.editorconfig`, `LICENSE` (GPLv3), the two GitHub
Actions workflows, `ARCHITECTURE.md`, and `adr/0001`. Fills **What this is** and
Project facts in `AGENTS.md`, replaces `README.md`, adds the first
`CONVENTIONS.md` rules, the first `CHANGELOG.md` entry, and the CI block. The
Deploy block stays empty until a remote exists.

## Out of scope

Chess rules, move generation, check and mate detection, wiring Stockfish into a
game, engine strength tuning, online multiplayer, accounts, persistence
contracts, visual design, sound, mobile-native packaging, and the GitHub remote
itself.

## Approval

- Gate: L2 bootstrap
- Approved by: Yauhen Pyl
- Approved at: 2026-11-16
- Artifact: adr/0001-blazor-wasm-on-github-pages.md sha256:00ad709092bebf0b8632a1153a27a578401a0daeaf10a9de282c349d9cb4c1d5
- Artifact: openspec/changes/bootstrap/proposal.md sha256:cee32f74aa13f98666b711c1979078cfe1a4265e04a045bfc28ed487da7d9ed3
- Artifact: openspec/changes/bootstrap/design.md sha256:54b1da66c46454d9b0ded9ab8c0b3622f2402b82157c720513d26e367e4f014b
- Words: "(none)"
