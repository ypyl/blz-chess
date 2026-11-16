# Play against Stockfish, at a level you pick

L1

## Why

The app installs and serves a page that says chess is coming. There is nothing to
play. The product in `AGENTS.md` promises a game against Stockfish in a tab, and
that game does not exist yet.

## What Changes

One page, one decision, then a game: the page lists opponent levels from "No idea
how to play" to "Expert", and choosing one starts a game against Stockfish at that
strength. The player moves pieces on the board, the computer answers, and the game
ends with a named result.

This is the whole first feature, not a fragment of it: a rules library, a game
session, the engine wiring, and the page. It is not split into a goal because
every candidate child leaves the product unable to play a game, and the template's
own split test says not to make children that ship nothing.

## Routing

0. **Observable behavior changes?** Yes. The app gains its first capability: a
   person can play chess against the computer at a chosen level.
1. **Undoable in one commit?** The change is a revert, no data exists, and the
   pages are not a published contract. The one-way part is the engine asset,
   which is a file and can be deleted.
2. **A machine can prove it correct?** Yes for the rules and the level ladder:
   perft counts on known positions, and deterministic ladder tests. The page
   interaction itself is checked by the browser smoke, which reads the same page
   a person would.
3. **Does anything outside depend on it?** No published contract. `adr/0001`
   already decided Blazor WASM, Stockfish, and single-threaded hosting; this
   change implements it and needs no new decision.

Level **L1**: behavior moves, nothing outside depends on it, and the engine choice
was already made.

## Capabilities

**Modified** `game`: created here. The whole capability is the page and the game.

## Impact

`src/BlzChess.Core` gains the rules and the game session, `src/BlzChess.Web` gains
the page and the engine, `wwwroot/engine` gains the vendored Stockfish build, and
`DESIGN.md` appears with the visual language. `README.md`, `CHANGELOG.md`, and
`ARCHITECTURE.md` are updated to match. `adr/0001` is untouched.

## Out of scope

Choosing a side, two players on one device, undo, hints, analysis or evaluation
bars, clocks, saved games, online play, takebacks, under-promotion, and every
variant. A refresh loses the game, by design.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-16
- Artifact: openspec/changes/play-against-stockfish/specs/game/spec.md sha256:f80c00146ff95d88ad3c5edbe37e9a7bf918bd444c700493228219d9112c468a
- Artifact: openspec/changes/play-against-stockfish/proposal.md sha256:2f5686d09be1633bd3e244f0ca41531a6884a8b60e992f20c2159f85a2283cbe
- Words: "(none)"
