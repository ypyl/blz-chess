# Tasks

## 1. Core

- [ ] 1.1 `GameFlow.Restart()`: fresh session at the current level, only while
      playing. Verified by tests for the level, the position, the empty move list,
      and the unchanged stage.
- [ ] 1.2 `Restart()` does nothing from the level list or after a finished game.
      Verified by two tests.

## 2. Page

- [ ] 2.1 The header action is "Restart" while playing and "New game" when the game
      is over, in one slot. Verified by reading the label in each state in the
      browser.
- [ ] 2.2 Restarting leaves the level name and the board's position unchanged.
      Verified by measuring the board's top edge before and after.

## 3. Docs

- [ ] 3.1 `CHANGELOG.md` names the restart. Verified by reading it.
- [ ] 3.2 `DESIGN.md` records the single-action header. Verified by reading it.

## 4. Verify and land

- [ ] 4.1 Every scenario in the delta spec has a test whose name contains its title,
      including the new scenarios. Verified by `scripts/check-change.sh` counting the
      scenarios.
- [ ] 4.2 `bash scripts/verify.sh` green, `node scripts/gate.mjs give-up --verify`
      exits 0, `review-change` run.
- [ ] 4.3 Archive, commit, push, watch `ci`, `deploy`, `smoke`, merge.
