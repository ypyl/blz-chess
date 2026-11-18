# Tasks

## 1. The sum, in Core

- [ ] 1.1 `Material.ValueOf` and `Material.Taken`, the scale in one place.
- [ ] 1.2 Tests: a pawn, a knight, a queen, a mixed tray, an empty tray, a promoted
      queen taken, and a take-back that lowers the total.

## 2. On the shelf

- [ ] 2.1 A span at the end of each tray, right-aligned by `margin-left: auto`.
- [ ] 2.2 No number for an empty tray.
- [ ] 2.3 The tray's label carries the total in words; the number is hidden from the
      accessibility tree.

## 3. Prove it in a browser

- [ ] 3.1 Playwright: take pieces, the numbers appear on the right of each shelf and
      match the pieces drawn.
- [ ] 3.2 Playwright: the board's box is unchanged by a capture, and the number's right
      edge sits inside the shelf.
- [ ] 3.3 Playwright: a take-back lowers the number; an empty tray shows none.
- [ ] 3.4 Screenshots: with scores, opening with none, phone.

## 4. Verify and land

- [ ] 4.1 `bash scripts/verify.sh`, with the commands and results recorded.
- [ ] 4.2 `DESIGN.md` gains the score, `CHANGELOG.md` the user-facing line.
- [ ] 4.3 `feat`, `chore: archive`, merge `--no-ff`, push, `ci`, `deploy`, `smoke` green.
