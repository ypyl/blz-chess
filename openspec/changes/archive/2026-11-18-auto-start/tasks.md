# Tasks

## 1. Core

- [x] 1.1 Remove `GameStage.Choosing`; `GameFlow` starts a game at the weakest level
      in its constructor. Verified by a test reading the stage, the level, and the
      position on a new flow.
- [x] 1.2 `GameFlow.NewGame()` starts a fresh game at the level shown and is the only
      way out of a finished game. Verified by a test after a real checkmate.
- [x] 1.3 `ChangeLevel` works whenever a game is shown, including after a result.
      Verified by a test that changes the level on a finished game.

## 2. Page

- [x] 2.1 Remove the chooser block and `ChooseLevel`. Verified by the page rendering a
      board on a cold load.
- [x] 2.2 The level list is enabled in every state and shows the level in play.
      Verified in the browser on load and after a restart.
- [x] 2.3 The deck and its CSS go with the chooser. Verified by no `.level` button
      remaining in the served page.

## 3. Docs

- [x] 3.1 `CHANGELOG.md` and `DESIGN.md`. Verified by reading both.

## 4. Verify and land

- [x] 4.1 Every scenario in the resulting spec has a test whose name contains its
      title, and tests for removed scenarios are removed. Verified by
      `scripts/check-change.sh` after the archive.
- [x] 4.2 `bash scripts/verify.sh`, `node scripts/gate.mjs auto-start --verify`,
      `review-change`.
- [ ] 4.3 Archive, commit, push, watch `ci`, `deploy`, `smoke`, merge.
