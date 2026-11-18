# Drag a piece to its square

L1

## Why

The board only moves a piece by tapping it and then tapping the destination. That is
the right way on a phone and the wrong way with a mouse: a player at a desk reaches
for the piece and drags it, and nothing happens. Every other place they have played
chess works that way, so the absence reads as the app being broken rather than as a
different habit.

## What Changes

A piece can be picked up and dropped on its destination. Pressing a piece of the
player's own lifts it, its legal destinations light up while it travels, and letting
go on one of them plays the move. Letting go anywhere else puts the piece back where
it came from and records nothing.

Tap-to-move stays exactly as it is, because a finger that taps must not be read as a
drag that failed. Both work on a mouse and on a finger, at any point where the player
is to move, and neither is offered while the computer is thinking.

## Routing

0. **Observable behavior changes?** Yes. A piece can now be moved a second way. The
   requirement "The player moves pieces on the board" names one way of moving, so it
   moves.
1. **Can it be undone easily?** Yes. One commit removes the pointer handlers and the
   hit test; no data, no published contract, no stored state.
2. **Can a machine prove it correct?** Yes, in two parts. The hit test (which square
   is under the pointer, on a board of any size) and the drop rule (a drop on a
   destination plays the move, a drop anywhere else does not) are pure functions
   tested in `DragTests`. The wiring from a real pointer to those functions is proven
   in a browser with Playwright, by mouse and with a touch context.
3. **Does anything outside depend on it?** No. The spec is the only contract, and this
   change moves it.

Level **L1**: observable behavior changes and a machine can prove it.

## Capabilities

`game`. The requirement "The player moves pieces on the board" gains the second way of
moving, and four scenarios.

## Impact

The board component gains pointer handling and a lifted piece. The hit test lands in
`BlzChess.Core` so it can be tested without a browser. The stylesheet gains the lifted
piece and stops the board from panning while a drag is in progress.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-18
- Artifact: openspec/changes/drag-to-move/specs/game/spec.md sha256:bee3cd7695484b3e7d1c56f74cd749551e295144e50c3a0304a26c675494c81d
- Artifact: openspec/changes/drag-to-move/proposal.md sha256:463acd80e40e22f9e0d372b00c65fe9f964c109bbcac7211a383242867d70a81
- Words: "(none)"
