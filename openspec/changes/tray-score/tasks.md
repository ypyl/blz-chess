# Tasks

## 1. The sum, in Core

- [x] 1.1 `Material.ValueOf` and `Material.Taken`, the scale in one place.
- [x] 1.2 Tests: a pawn, a knight, a queen, a mixed tray, an empty tray, a promoted
      queen taken, and a take-back that lowers the total.

## 2. On the shelf

- [x] 2.1 A span at the end of each tray, right-aligned by `margin-left: auto`.
- [x] 2.2 No number for an empty tray.
- [x] 2.3 The tray's label carries the total in words; the number is hidden from the
      accessibility tree.

## 3. Prove it in a browser

- [x] 3.1 Playwright: take pieces, the numbers appear on the right of each shelf and
      match the pieces drawn.
- [x] 3.2 Playwright: the board's box is unchanged by a capture, and the number's right
      edge sits inside the shelf.
- [x] 3.3 Playwright: a take-back lowers the number; an empty tray shows none.
- [x] 3.4 Screenshots: with scores, opening with none, phone.

## 4. Verify and land

- [x] 4.1 `bash scripts/verify.sh`, with the commands and results recorded.
- [x] 4.2 `DESIGN.md` gains the score, `CHANGELOG.md` the user-facing line.
- [x] 4.3 `feat`, `chore: archive`, merge `--no-ff`, push, `ci`, `deploy`, `smoke` green.

## Results

- `bash scripts/verify.sh`: lint and build clean, 95 tests pass.
- Playwright, 1280x800: after taking a black pawn the player's shelf read "1" with the
  label "You have taken: black pawn, worth 1.", the number 4.8px inside the shelf's right
  edge, and the board box was `126,368,467` before and after. Undo removed the number and
  the shelf read "nothing yet" again; the board did not move.
- After a fuller game the shelves read 20 and 28 over four and ten pieces, matching the
  labels "worth 20" and "worth 28". Opening board: no number on either shelf.
- 390x844: the number sits 5px inside the shelf, shelf 36px, board 374, no scroll.
- Screenshots: `31-tray-scores-desktop`, `32-tray-scores-phone`.
