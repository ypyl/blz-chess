# Tasks

## 1. Core

- [ ] 1.1 `GameSession.ChangeLevel(Level)` with the level readable and settable.
      Verified by a test that reads the level back.
- [ ] 1.2 `GameFlow.ChangeLevel(Level)`, refused unless a game is in progress.
      Verified by tests for the allowed and both refused stages.
- [ ] 1.3 The position, the move list, and the side to move survive the change.
      Verified by the scenario test.

## 2. Page

- [ ] 2.1 The header's first slot is a level list while the header exists, disabled
      once the game is over. Verified in the browser: the level changes mid-game and
      the control is disabled after a result.
- [ ] 2.2 Changing the level does not move the board. Verified by measuring its top
      edge before and after.

## 3. Docs

- [ ] 3.1 `CHANGELOG.md` and `DESIGN.md`. Verified by reading both.

## 4. Verify and land

- [ ] 4.1 Every scenario has a test whose name contains its title. Verified by
      `scripts/check-change.sh` after the archive.
- [ ] 4.2 `bash scripts/verify.sh`, `node scripts/gate.mjs change-level-midgame
      --verify`, `review-change`.
- [ ] 4.3 Archive, commit, push, watch `ci`, `deploy`, `smoke`, merge.
