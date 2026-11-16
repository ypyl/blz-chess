# Tasks

## 1. Signal, then fix

- [x] 1.1 Capture the failing signal. Done: reading each square's `src` on the
      board reports `pieces/wK.svg` on b1 and g1, where the knights stand.
- [x] 1.2 Move the mapping into `PieceFiles` with an explicit letter per type.
      Verified by `TwelvePiecesTwelveFiles` and one case per piece.
- [x] 1.3 Add the square separation in `app.css` and record it in `DESIGN.md`.
      Verified by the screenshot and by the board still measuring square.
- [x] 1.4 `bash scripts/verify.sh`. Done: 48 tests, checks green.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
- [ ] 2.2 Confirm the deployed board shows knights and separated cells. Verified
      by the browser reading b1 as `pieces/wN.svg` on the live site.
