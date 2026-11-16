# Design: rules, ladder, engine, page

`adr/0001` decided Blazor WASM, Stockfish in a Web Worker, static hosting. This is
how it is built.

## Core, and why the rules live there

`BlzChess.Core` holds everything a machine can test without a browser:

| Type | Job |
|---|---|
| `Position` | piece placement, side to move, castling rights, en passant square, halfmove clock, fullmove number; parses and writes FEN |
| `MoveGenerator` | every legal move for the side to move, generated pseudo-legally then filtered by "does this leave my king attacked" |
| `Game` | applies a move, keeps the move list, counts repetitions, and answers the outcome |
| `Level` | name, skill, depth, move time, and random-move probability |
| `GameSession` | player is White, holds the `Game`, asks `IEngine` for the computer's reply, refuses anything illegal |

The Razor page renders `GameSession` and calls it. No chess rule is allowed to
live in a component, because a rule tested through a browser is a rule tested by
eye.

## Levels

Stockfish's own floor is far above "does not know the rules", so weakness is made
in two places: what we ask the engine for, and whether we ask it at all.

| # | Name | skill | depth | move time | random legal move |
|---|---|---|---|---|---|
| 1 | No idea how to play | 0 | 1 | 100 ms | 80% |
| 2 | Just learning | 0 | 2 | 150 ms | 50% |
| 3 | Beginner | 1 | 4 | 200 ms | 25% |
| 4 | Casual | 5 | 6 | 400 ms | 5% |
| 5 | Club player | 12 | 10 | 800 ms | 0 |
| 6 | Expert | 20 | 16 | 1500 ms | 0 |

"Random" is a legal move picked by seed, not a random move from a list of all
moves: the weakest level must still be a chess player who follows the rules and
sometimes finds a capture. The seed comes from the session so a test can pin it.

Monotonicity is a property of the table, and the table has a test: skill, depth,
and time never decrease as the level rises, and the random probability never
increases. That is what "a stronger level is never weaker" means in code.

## The engine

`wwwroot/engine/stockfish-19-lite-single.js` and `.wasm`, vendored from
`stockfish@19.0.1` (1.8 MB, lite network, single-threaded: threaded builds need
COOP/COEP, which Pages cannot send). Version and sha256 live in
`EngineInfo.cs` next to the path, so an upgrade is a visible diff.

`wwwroot/engine/engine.js` owns the Worker and the UCI conversation: `uci`,
`setoption name Skill Level`, `position fen`, `go depth N movetime M`, and one
promise per `bestmove`. The C# side sees one method, `BestMoveAsync(fen, level)`,
behind `IEngine`. Tests use a fake implementation, so the test suite never needs a
browser.

The engine is fetched on the first computer move, after the board is already
interactive, and the service worker caches it. A game at level 1 often never loads
it at all.

## The page

One route, `/`. Three states, one component: `Choosing` (the level list),
`Playing` (board, status line, New game), `Finished` (result named above a board
that refuses input). Click a piece, then a destination; the second click on a legal
square makes the move. Promotion is always a queen, which the spec states, so
there is no dialog.

While the engine thinks the status line says so and the board refuses input; the
reply arrives as a task on the UI thread, so the tab stays responsive.

## Pieces and look

Twelve SVG pieces from the Cburnett set, which is CC BY-SA 3.0 and compatible with
this repository's GPLv3, with attribution in the README and `DESIGN.md`. `DESIGN.md`
holds the palette and the board proportions; the spec holds none of it, because a
spec that names colors cannot survive a redesign.

## Verification

- Perft counts on the start position to depth 4 (`197281`), Kiwipete to depth 2,
  and a position covering castling and en passant.
- Ladder tests for monotonicity and for the two level scenarios in the spec.
- Session tests with a fake engine: illegal move refused, checkmate ends the game,
  new game returns to the level list.
- A browser smoke on the live site: pick Expert, play e2-e4, wait for a reply.
