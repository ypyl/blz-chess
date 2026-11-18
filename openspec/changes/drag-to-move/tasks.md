# Tasks

## 1. The hit test, before anything draws

- [x] 1.1 `BoardGeometry.SquareAt(x, y, side)` in `BlzChess.Core`: file from `x`,
      rank from `y` with rank 8 at the top, `Squares.None` outside the board.
- [x] 1.2 `DragTests` proves the hit test at both board sizes (544 and 222), at all
      four corners, on an edge point, and past the edge.
- [x] 1.3 `DragTests` proves the drop rule with `GameFlow`: a drop on a destination
      plays the move and records it, a drop on a square that is not a destination
      records nothing, and a press on a black piece offers no destination.
- [x] 1.4 `DragTests` proves the scenario "a tap still moves a piece without
      dragging" through the click path, so the old way is not lost.

## 2. The pointer

- [x] 2.1 `wwwroot/js/pointer.js`: board rectangle, set and release pointer capture.
- [x] 2.2 Pointer handlers on the board: press, threshold, move, release, cancel.
- [x] 2.3 The lifted piece, centred on the pointer and invisible to pointers.
- [x] 2.4 `touch-action: none` on the board with the `shortcut:` marker from
      `design.md`.
- [x] 2.5 The engine's arrow keeps `pointer-events: none`, so a press over it reaches
      the square beneath.

## 3. Prove it in a browser

- [x] 3.1 Playwright, mouse: drag e2 to e4, the pawn is on e4, the move is recorded,
      and the computer answers.
- [x] 3.2 Playwright, mouse: drag a pawn to a square it may not reach, the pawn is
      back on its square and no move is recorded.
- [x] 3.3 Playwright with a touch context: the same drag with a finger.
- [x] 3.4 Playwright: tap a piece and tap a destination, the move plays as before.
- [x] 3.5 Screenshots of a piece mid-drag and of a finished drag.

## 4. Verify and land

- [x] 4.1 `bash scripts/verify.sh`: lint, build, and 69 tests pass.
- [x] 4.4 A press that awaited the board's rectangle before arming the drag lost a fast
      flick; the press is now synchronous and reads the square from its own button.
- [x] 4.2 `DESIGN.md` gains the drag, `CHANGELOG.md` gains the user-facing line.
- [x] 4.3 Commit `plan` was frozen before apply; then `feat`, `chore: archive`,
      merge `--no-ff`, push, and `ci`, `deploy`, `smoke` green.

## Results

Browser, `dotnet run` on 127.0.0.1:5199, Playwright:

- Mouse drag e2 to e4: ghost and 2 destination markers mid-drag, pawn on e4 after,
  move recorded, computer answered.
- Illegal drop d2 to d5: pawn on d2, d5 empty, no marker left, still the player's turn.
- Drop outside the board (d2, released 260x200 to the upper left): piece returned,
  still the player's turn.
- Two-step flick g1 to f3: the knight moved, so a drag faster than the round trip is
  not lost.
- Touch (CDP `Input.dispatchTouchEvent`), d2 to d4: ghost and 2 markers mid-drag, pawn
  on d4 after.
- Tap b1 then c3: the knight moved, so the click path is intact. This is the check that
  caught `setPointerCapture` retargeting the click to the board and taking tap-to-move
  away; capture now happens when a press becomes a drag, not on the press.
- 390x844: ghost 47px on a 47px cell, inside the board, no scroll (844/844).
- Console errors: 0.
