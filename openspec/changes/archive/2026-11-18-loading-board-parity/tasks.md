# Tasks

## 1. Fix and prove

- [x] 1.1 Regenerate the loading squares with the board's parity. Done.
- [x] 1.2 Read the corner colours in the browser. Done: loading `a8` is
      `rgb(240, 238, 214)` and `a1` is `rgb(125, 148, 93)`, matching the game board's
      `a8` and `a1` exactly.
- [x] 1.3 Add the parity guard to `scripts/verify.sh`. Done, and it fails on a copy
      with one square flipped, naming that square.
- [x] 1.4 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
