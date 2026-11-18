# Tasks

## 1. Move and prove

- [x] 1.1 The status moves under the board with a reserved height; the header keeps
      the level and the actions. Done.
- [x] 1.2 The loading view mirrors the same shape. Done.
- [x] 1.3 The board is unmoved by any status. Done: `87.2` for "Your move", "Thinking",
      checkmate, and insufficient material, all identical.
- [x] 1.4 Widen the height cap for the new slot and re-measure every viewport. Done:
      no scroll at 360x740, 390x844, 412x915, 844x390, 915x412, and the desktop board
      is still 544; the loading view holds at landscape too.
- [x] 1.5 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
