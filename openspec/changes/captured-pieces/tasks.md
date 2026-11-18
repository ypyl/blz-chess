# Tasks

## 1. The record, in Core

- [ ] 1.1 `Game.Captured`: the pieces taken, in order, filled where a move is applied.
- [ ] 1.2 Read the taken piece from the position before the move, including the pawn a
      capture in passing removes.
- [ ] 1.3 Rebuild the list on the same replay a take-back already runs, so the two
      cannot disagree.
- [ ] 1.4 Tests: a capture, a capture in passing, a take-back, a captured promoted
      queen, and an empty list at the opening.

## 2. The trays

- [ ] 2.1 One reserved line above the board for the computer's captures, one below for
      the player's, always present.
- [ ] 2.2 The board's height cap takes the trays and their gaps out of the viewport, in
      one variable.
- [ ] 2.3 The loading screen reserves the same two lines.
- [ ] 2.4 A label in words for each tray; the pictures decorative.

## 3. Prove it in a browser

- [ ] 3.1 Playwright: take a piece, it appears in the right tray and leaves the board.
- [ ] 3.2 Playwright: the board's box is identical before and after the capture, at
      1280x800 and 390x844.
- [ ] 3.3 Playwright: the board's box is identical before the takeover and after it.
- [ ] 3.4 Playwright: a take-back empties the tray.
- [ ] 3.5 Screenshots: an empty tray at the opening, a tray with pieces, phone.

## 4. Verify and land

- [ ] 4.1 `bash scripts/verify.sh`, with the commands and results recorded.
- [ ] 4.2 `DESIGN.md` gains the trays, `CHANGELOG.md` the user-facing line.
- [ ] 4.3 `feat`, `chore: archive`, merge `--no-ff`, push, `ci`, `deploy`, `smoke` green.
