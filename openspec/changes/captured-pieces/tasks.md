# Tasks

## 1. The record, in Core

- [x] 1.1 `Game.Captured`: the pieces taken, in order, filled where a move is applied.
- [x] 1.2 Read the taken piece from the position before the move, including the pawn a
      capture in passing removes.
- [x] 1.3 Rebuild the list on the same replay a take-back already runs, so the two
      cannot disagree.
- [x] 1.4 Tests: a capture, a capture in passing, a take-back, a captured promoted
      queen, and an empty list at the opening.

## 2. The trays

- [x] 2.1 One reserved line above the board for the computer's captures, one below for
      the player's, always present.
- [x] 2.2 The board's height cap takes the trays and their gaps out of the viewport, in
      one variable.
- [x] 2.3 The loading screen reserves the same two lines.
- [x] 2.4 A label in words for each tray; the pictures decorative.

## 3. Prove it in a browser

- [x] 3.1 Playwright: take a piece, it appears in the right tray and leaves the board.
- [x] 3.2 Playwright: the board's box is identical before and after the capture, at
      1280x800 and 390x844.
- [x] 3.3 Playwright: the board's box is identical before the takeover and after it.
- [x] 3.4 Playwright: a take-back empties the tray.
- [x] 3.5 Screenshots: an empty tray at the opening, a tray with pieces, phone.

## 4. Verify and land

- [x] 4.1 `bash scripts/verify.sh`, with the commands and results recorded.
- [x] 4.2 `DESIGN.md` gains the trays, `CHANGELOG.md` the user-facing line.
- [x] 4.3 `feat`, `chore: archive`, merge `--no-ff`, push, `ci`, `deploy`, `smoke` green.

## Results

- `bash scripts/verify.sh`: lint and build clean, 74 tests pass.
- A capture, by Playwright against `dotnet run`, at 1280x800: the player took the
  black queen on b6. Trays read "The computer has taken: white pawn, white knight,
  white bishop" and "You have taken: black pawn, black queen", drawn in that order.
  The board's box was `126,368,467,467` before and after the capture, identical.
- Take back: the queen left the tray (2 pieces to 1), and the board's box did not move.
- Takeover: with `_framework` delayed by 3.5s, the loading board and the game board
  both measured `top 126, left 368, 467x467`, and both views reserved 2 trays.
- 390x844: board 374, trays 28 each, no scroll (844/844). 844x390: board 137, no
  scroll (390/390). 1280x800: board 544, no scroll (800/800).
- Screenshots: `20-trays-with-pieces`, `21-trays-phone`, `22-trays-landscape`,
  `23-trays-empty-opening` in `.tmp/testing`.
