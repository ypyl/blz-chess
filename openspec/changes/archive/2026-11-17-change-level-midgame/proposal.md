# Change the opponent level during a game

L1

## Why

The level is chosen once, before the first move, and after that the only way to
change it is a restart, which throws the game away. A player who has misjudged the
level has to abandon a game to fix it, which is exactly when the move they are
looking at is the interesting part.

## What Changes

The level name in the header becomes the choice itself: a list of the six levels,
usable while a game is in progress. Choosing another level changes the opponent for
its next move and leaves everything else alone: same position, same moves, same side
to move, same result to come. Once a game has finished the list stays visible so the
result reads in context, but it no longer changes anything.

## Routing

0. **Observable behavior changes?** Yes. The opponent can change mid-game, and the
   level is now a control rather than a label.
1. **Undoable in one commit?** Yes. One property on the session, one method on the
   flow, one control in the header. No data, no published contract.
2. **A machine can prove it correct?** Yes: flow and session tests assert the
   position, the move list, the side to move, and the level handed to the engine;
   the browser check reads the board's top edge and the position around the change.
3. **Does anything outside depend on it?** `openspec/specs/game/spec.md`, which gains
   one requirement through this delta.

Level **L1**: behavior moves, so the delta spec is the gate.

## Capabilities

**Modified** `game`: one requirement is added. Existing requirements about choosing
a level before the first move and about restarting are unchanged.

## Impact

`BlzChess.Core`: `GameSession` can change its level, and `GameFlow` exposes that with
a stage guard. `Pages/Play.razor` renders the level as a list in the header.
`DESIGN.md` and `CHANGELOG.md` follow.

## Out of scope

Changing the level for a move already being thought about, per-level clocks or time
controls, and remembering a level for the next game. A restart still returns to the
same level, and a new game still asks again.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-17
- Artifact: openspec/changes/change-level-midgame/specs/game/spec.md sha256:f7ee767ab1112b4afe3bcf6af184a89e206d4b8d00ce2817b1d2ab27be182053
- Artifact: openspec/changes/change-level-midgame/proposal.md sha256:67b2104a0411a35c5480a3b7312e9c9642f8b7006737a23666e8b15d70112654
- Words: "(none)"
