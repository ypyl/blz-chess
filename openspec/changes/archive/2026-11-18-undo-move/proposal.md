# Take back the last move

L1

## Why

One wrong move costs the whole game. The player's only options today are to restart,
which throws the whole game away, or to play on from a position they already know is
lost. A casual opponent lets you say "wait, let me try that again", and the page does
not.

## What Changes

The header gains a take-back, next to Restart, offered while a game is running. It
removes the player's last move and the computer's answer together, because giving
back only the player's move would hand the computer a free extra reply. The board,
the move history, and the repetition state return to exactly what they were, and it
is the player's turn.

At the opening there is nothing to take back and the control is inert. Once a game
has finished the control is gone, so a result stays a result.

## Routing

0. **Observable behavior changes?** Yes. A move can be un-played, which the product
   could not do before.
1. **Undoable in one commit?** Yes. One method on the game, one on the flow, one
   control in the header. No data, no published contract.
2. **Can a machine prove it correct?** Yes: tests replay a position, take a move
   back, and compare the FEN, the move list, the side to move, and the repetition
   behaviour with what was there before; a repetition test proves the rebuilt state
   is real and not cosmetic.
3. **Does anything outside depend on it?** `openspec/specs/game/spec.md`, which gains
   one requirement through this delta.

Level **L1**: behavior moves, so the delta spec is the gate.

## Capabilities

**Modified** `game`: one requirement is added.

## Impact

`BlzChess.Core`: `Game` can drop moves from its history and rebuild the position by
replaying what is left; `GameSession` and `GameFlow` guard it. `Pages/Play.razor` and
the header CSS gain the control. `DESIGN.md` and `CHANGELOG.md` follow.

## Out of scope

Taking back more than one turn at a time (each press takes one turn, and pressing
again takes the one before), taking back a move in a finished game, redo, a
confirmation before the take-back, and counting how often a player takes back. A
player can press twice, which is two turns, and that is enough for a casual board.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-18
- Artifact: openspec/changes/undo-move/specs/game/spec.md sha256:962a4e262892217d61a099b683dfc5e406487c79f7a53656e7c228720aeb7684
- Artifact: openspec/changes/undo-move/proposal.md sha256:816c298d81b1319754ec7c75df6502d84bf01f6756cd1cf820734651f4469b14
- Words: "(none)"
