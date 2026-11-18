# Tasks

## 1. Draw and prove

- [x] 1.1 Compute the arrow in the page from the move's squares, with the tip held
      inside the destination square. Done: `ArrowGeometry`.
- [x] 1.2 Render it as an overlay that takes no clicks. Done: `pointer-events: none`.
- [x] 1.3 Clear it on the player's move, a take-back, a restart, and a new game. Done,
      and verified for the take-back in the browser.
- [x] 1.4 Measure the geometry live. Done: the reply e7-e5 drew `450,150 -> 450,198`
      with the head at `450,232`, the exact square centres, and the arrow stayed
      through the player's turn.
- [x] 1.5 The squares beneath remain clickable with the arrow up. Done: further moves
      were played while the arrow was on the board.
- [x] 1.6 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
