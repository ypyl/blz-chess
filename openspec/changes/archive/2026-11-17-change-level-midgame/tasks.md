# Tasks

## 1. Core

- [x] 1.1 `GameSession.ChangeLevel(Level)` with the level readable and settable.
      Verified by a test that reads the level back.
- [x] 1.2 `GameFlow.ChangeLevel(Level)`, refused unless a game is in progress.
      Verified by tests for the allowed and both refused stages.
- [x] 1.3 The position, the move list, and the side to move survive the change.
      Verified by the scenario test.

## 2. Page

- [x] 2.1 The header's first slot is a level list while the header exists, disabled
      once the game is over. Done: the browser switched Casual to Expert mid-game and
      the list was live; the disabled state comes from the stage and is covered by
      `ChangingTheLevelIsOfferedOnlyWhileAGameIsInProgress`.
- [x] 2.2 Changing the level does not move the board. Verified by measuring its top
      edge before and after.

## 3. Docs

- [x] 3.1 `CHANGELOG.md` and `DESIGN.md`. Verified by reading both.

## 4. Verify and land

- [x] 4.1 Every scenario has a test whose name contains its title. Verified by
      `scripts/check-change.sh` after the archive.
- [x] 4.2 `bash scripts/verify.sh`, `node scripts/gate.mjs change-level-midgame
      --verify`, `review-change`.
- [ ] 4.3 Archive, commit, push, watch `ci`, `deploy`, `smoke`, merge.
