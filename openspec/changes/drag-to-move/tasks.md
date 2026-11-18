# Tasks

## 1. The hit test, before anything draws

- [ ] 1.1 `BoardGeometry.SquareAt(x, y, side)` in `BlzChess.Core`: file from `x`,
      rank from `y` with rank 8 at the top, `Squares.None` outside the board.
- [ ] 1.2 `DragTests` proves the hit test at both board sizes (544 and 222), at all
      four corners, on an edge point, and past the edge.
- [ ] 1.3 `DragTests` proves the drop rule with `GameFlow`: a drop on a destination
      plays the move and records it, a drop on a square that is not a destination
      records nothing, and a press on a black piece offers no destination.
- [ ] 1.4 `DragTests` proves the scenario "a tap still moves a piece without
      dragging" through the click path, so the old way is not lost.

## 2. The pointer

- [ ] 2.1 `wwwroot/js/pointer.js`: board rectangle, set and release pointer capture.
- [ ] 2.2 Pointer handlers on the board: press, threshold, move, release, cancel.
- [ ] 2.3 The lifted piece, centred on the pointer and invisible to pointers.
- [ ] 2.4 `touch-action: none` on the board with the `shortcut:` marker from
      `design.md`.
- [ ] 2.5 The engine's arrow keeps `pointer-events: none`, so a press over it reaches
      the square beneath.

## 3. Prove it in a browser

- [ ] 3.1 Playwright, mouse: drag e2 to e4, the pawn is on e4, the move is recorded,
      and the computer answers.
- [ ] 3.2 Playwright, mouse: drag a pawn to a square it may not reach, the pawn is
      back on its square and no move is recorded.
- [ ] 3.3 Playwright with a touch context: the same drag with a finger.
- [ ] 3.4 Playwright: tap a piece and tap a destination, the move plays as before.
- [ ] 3.5 Screenshots of a piece mid-drag and of a finished drag.

## 4. Verify and land

- [ ] 4.1 `bash scripts/verify.sh`, with the exact commands and results recorded.
- [ ] 4.2 `DESIGN.md` gains the drag, `CHANGELOG.md` gains the user-facing line.
- [ ] 4.3 Commit `plan` was frozen before apply; then `feat`, `chore: archive`,
      merge `--no-ff`, push, and `ci`, `deploy`, `smoke` green.
