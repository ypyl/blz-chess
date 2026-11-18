# Tasks

## 1. Move and prove by geometry

- [x] 1.1 Move the overlay inside the board and pin its view box to the board's box.
      Done.
- [x] 1.2 Prove the overlay's screen box equals the board's. Done: identical to the
      pixel at 390x844, 844x390, and 1280x800.
- [x] 1.3 Prove the arrow points where the pieces are. Done: the shaft starts on the
      emptied from-square (`c7`) and the tip lands inside the destination (`c5`, black
      pawn).
- [x] 1.4 Screenshot it. Done: `.tmp/testing/13-arrow-inside-board.png`.
- [x] 1.5 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
