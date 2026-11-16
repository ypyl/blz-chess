# Tasks

## 1. Fix and prove

- [x] 1.1 Rewrite the base href before publish, in CI only. Verified by publishing
      locally after the same rewrite: `dist/wwwroot/index.html` reports
      `/blz-chess/` and the manifest still matches.
- [x] 1.2 Add the hash check to the deploy job. Verified locally: it reports 62
      assets matching, and it fails when a published file is edited afterwards.
- [x] 1.3 Keep `404.html` as a post-publish copy. Verified by reading the order of
      the steps.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
- [ ] 2.2 Confirm the deployed worker installs. Verified by the browser reporting
      `activated` and a page controller.
