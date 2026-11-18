# Tasks

## 1. Remove and prove

- [x] 1.1 Make the status slot a third invisible placeholder and delete the loading
      text rules. Done.
- [x] 1.2 Prove the page carries no text while loading. Done: `innerText` is empty
      with the runtime stalled and pieces placed.
- [x] 1.3 Prove the board did not move. Done: top `107.2`, size `544` before and
      after the change.
- [x] 1.4 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
