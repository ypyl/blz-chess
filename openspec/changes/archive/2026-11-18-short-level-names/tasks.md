# Tasks

- [x] 1.1 Rename in `Level.All`: Random, Learner, Beginner, Casual, Club, Expert.
- [x] 1.2 The loading placeholder and `README.md` carry the new names.
- [x] 1.3 Update the assertions that name a level; the scenario titles do not change.
- [x] 1.4 Measure the dropdown before and after, at 390x844 and 1280x800.
- [x] 1.5 `bash scripts/verify.sh`.
- [x] 1.6 `feat`, `chore: archive`, merge `--no-ff`, push, `ci`, `deploy`, `smoke` green.

## Results

- `bash scripts/verify.sh`: lint and build clean, 95 tests pass.
- Playwright, 1280x800: the dropdown is 107px wide, from 188px, and the options read
  Random, Learner, Beginner, Casual, Club, Expert.
- Playwright, 390x844: the dropdown is 107px with 45px of free space before the action
  buttons, from 141px and 12px. The header is still 40px, the board 374px, and the page
  does not scroll.
- Takeover, with the framework delayed 3s: the loading placeholder and the real select are
  both content-sized with the same word (101px and 107px, a select's arrow accounts for
  the difference), and the board is `127,544` before and after in both views.
- Screenshots: `35-short-levels-phone`, `36-short-levels-desktop`.
