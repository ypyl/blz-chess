# Tasks

## 1. Signal, then fix

- [x] 1.1 Capture the failing signal: register the worker on the deployed site and
      read its state. Done: `installing` then `redundant`.
- [x] 1.2 Find the asset that 404s. Done: every URL in `service-worker-assets.js`
      answers 200 except `.nojekyll`.
- [x] 1.3 Remove `.nojekyll` from `wwwroot`. Done.
- [x] 1.4 Add the dotfile checks to `scripts/verify.sh` and the deploy workflow.
      Verified by `bash scripts/verify.sh` reporting no dotfiles, and by the
      deploy step passing on the next run.
- [x] 1.5 Prove the worker installs. Verified by the deployed site reporting
      `activated` and a controller on the page.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, and watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
