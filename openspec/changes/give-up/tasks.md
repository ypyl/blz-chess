# Tasks

## 1. Core

- [x] 1.1 `GameFlow.Restart()`: fresh session at the current level, only while
      playing. Done: `RestartingStartsTheSameLevelAgain` and `ARestartRecordsNoResult`.
- [x] 1.2 `Restart()` does nothing from the level list or after a finished game.
      Done: `RestartIsOfferedOnlyWhileAGameIsInProgress`, which drives fool's mate
      through the flow so the stage really reaches `Finished`.

## 2. Page

- [x] 2.1 The header action is "Restart" while playing and "New game" when the game
      is over, in one slot. Done: the browser reads "Restart" while playing; the
      finished label is one conditional expression, and the `Finished` stage behind
      it is covered by the flow test, not by a browser click, because a game cannot
      be won on demand.
- [x] 2.2 Restarting leaves the level name and the board's position unchanged. Done:
      after a move and a restart the browser reports level `Casual`, `e2` a pawn,
      `e4` empty, and the board's top edge at `107.2` before and after.

## 3. Docs

- [x] 3.1 `CHANGELOG.md` names the restart. Verified by reading it.
- [x] 3.2 `DESIGN.md` records the single-action header. Verified by reading it.

## 4. Verify and land

- [x] 4.1 Every scenario in the delta spec has a test whose name contains its title,
      including the new scenarios. Verified by `scripts/check-change.sh` counting the
      scenarios.
- [x] 4.2 `bash scripts/verify.sh` green, `node scripts/gate.mjs give-up --verify`
      exits 0, `review-change` run.
- [ ] 4.3 Archive, commit, push, watch `ci`, `deploy`, `smoke`, merge.
