# Put the material score on the trays

L1

## Why

The trays show which pieces are gone. They do not say who is ahead, which is the
question a player actually asks when they look at them: three pawns and a knight one way,
a rook the other, and the answer needs arithmetic nobody does mid-game. The count is one
number per side and the pieces are already on the page to make it.

## What Changes

Each tray carries the total value of the pieces on it, right-aligned on the shelf, where
a chess player already looks for a material count. A pawn counts 1, a knight and a bishop
3, a rook 5, a queen 9, and a king never appears because a king is never taken.

A tray with nothing in it shows no number rather than a zero, so an opening board is as
quiet as it is today. The number sits inside the line the tray already reserves, so
nothing about the board's place or size changes, before or after a piece is taken.

Taking a move back takes the number back with the piece. A pawn that became a queen and
was then taken counts 9, not 1, because the tray records the piece that was taken and not
the piece it started life as.

## Routing

0. **Observable behavior changes?** Yes. The page shows a number it does not show today,
   and the requirement "The page shows the pieces each side has taken" grows to include
   it.
1. **Can it be undone easily?** Yes: one commit, no stored data, no contract.
2. **Can a machine prove it correct?** Yes. The sum is arithmetic over the list the
   previous change already records and tests, so the cases that matter (a pawn, a queen,
   an empty tray, a promoted queen, a take-back) are unit tests. That the number is on
   the right of the shelf and does not move the board is measured in a browser.
3. **Does anything outside depend on it?** No.

Level **L1**: observable behavior changes and a machine can prove it.

## Capabilities

`game`. The existing requirement gains the value and three scenarios.

## Impact

`Material` in `BlzChess.Core` holds the scale and the sum, so the numbers are tested away
from the page. Each tray gains a span, right-aligned by the row it is already in, and its
label gains the total so a screen reader gets the number too.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-18
- Artifact: openspec/changes/tray-score/specs/game/spec.md sha256:51478f005ff79b784a1475a1bfb1fab75a8fdc250b210136b82c5c1afde94d25
- Artifact: openspec/changes/tray-score/proposal.md sha256:28979ae3197cfd9d72bfb5929cd416aa14ae371519716d062c1e3b4ca0417c9b
- Words: "(none)"
