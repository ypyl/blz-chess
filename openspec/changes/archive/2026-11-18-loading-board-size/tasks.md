# Tasks

## 1. Match and prove

- [x] 1.1 Give the loading board the game board's geometry: shell width, square, one
      radius, one shadow, no gap. Done.
- [x] 1.2 Put the loading view in the game's shell with placeholder header slots and
      the percentage in the status slot. Done.
- [x] 1.3 Measure both states at both widths. Done: `544x544` at `left 368, top 107.2`
      on desktop and `358.8` wide at `top 261.8` on a 390-wide phone, identical in the
      loading state and the game state.
- [x] 1.4 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
