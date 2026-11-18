# Show a suggested move when the player asks for one

L1

## Why

A beginner reaches a position, sees nothing, and moves a pawn to have moved. The board
shows every legal destination, which answers "where may this piece go" and not "what
should I play". The engine in the tab knows the answer to the second question already,
because it is the same engine that beats them.

This is the teaching the product has been missing, and it is one switch: on, and every
one of the player's turns carries a suggestion; off, and the page is exactly what it was.

## What Changes

A switch in the header, "Hints", next to Undo and Restart, off when the page loads. While
it is on and it is the player's turn, the page shows the move the engine considers best
in that position, as a dashed arrow on the board, distinct from the solid arrow that
shows the computer's last move. When the player moves or the computer starts thinking,
the arrow for the old position goes; when the computer has answered, a suggestion for
the new position arrives.

The suggestion is always the engine's best move at full strength, whatever level the
opponent is playing at. A weak opponent must not mean a weak coach: asking the engine
through the level ladder would sometimes recommend the blunder that makes "No idea how
to play" feel like a first game.

Switching hints on changes nothing else. It asks the engine a question, draws the
answer, and the game, the result, the level, and the moves already played are untouched.
A suggestion is never a move.

Hints are not remembered across a refresh, the way the level is not, so the page keeps
its promise that nothing is stored.

## Routing

0. **Observable behavior changes?** Yes. The page gains a switch and a suggestion drawn
   on the board. A new requirement, so the spec moves.
1. **Can it be undone easily?** Yes: one branch, no stored data, no contract.
2. **Can a machine prove it correct?** Yes, mostly without a browser. The decision logic
   lives in `GameFlow`: off at the start, asked for at full strength regardless of the
   opponent's level, always a legal move for the player, cleared when the switch goes
   off or the computer thinks, refreshed after the computer answers, and never able to
   change the position. Those are unit tests with a scripted engine. What a test cannot
   say is that the arrow is drawn and is not confused with the computer's arrow; that
   is measured in a browser, both arrows read from the page at once.
3. **Does anything outside depend on it?** No. The spec is the only contract, and this
   change extends it.

Level **L1**: observable behavior changes and a machine can prove it.

## Capabilities

`game`. A new requirement, "The player can see a suggested move", with seven scenarios.

## Impact

`Level` gains the coach settings, kept out of the level list the page offers. `GameFlow`
gains the switch and the request, with a token so an answer that arrives after the
player has already moved is discarded instead of drawn. The page gains a header switch,
a second arrow overlay, and a line a screen reader can announce. The engine module is
untouched: `bestMove` already returns what is needed.

## Assumptions

- The suggestion is one move, the best one, not a ranked list. Three arrows on an
  opening board are noise, and `MultiPV` would mean parsing the engine's `info` lines,
  which this change does not need.
- The switch is a per-session preference. Remembering it in the browser's storage would
  be a store, which is a bigger decision than this change.
- No evaluation number is shown. "Nf3, plus 0.31" invites a question the page cannot
  answer, and it is analysis, which the product says it is not.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-18
- Artifact: openspec/changes/suggested-moves/specs/game/spec.md sha256:f294b77fef77466a92c49c2c85c9ae0e072b967a2a6440473d617988f883f20f
- Artifact: openspec/changes/suggested-moves/proposal.md sha256:f287757056561e4c4142446e74589b411e622311c5fc380752af1f4ae16f32bb
- Words: "(none)"
