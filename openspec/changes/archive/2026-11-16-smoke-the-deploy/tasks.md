# Tasks

## 1. Do

- [x] 1.1 Add the `smoke` job after `deploy`. Verified by the job passing on the
      hosted run and failing when a fetched asset is renamed by hand.
- [x] 1.2 Remove the template's `About` link. Verified by
      `grep -r learn.microsoft.com src/ --exclude-dir=bin --exclude-dir=obj`
      finding nothing.
- [x] 1.3 Ignore `.playwright-cli/` and remove the committed file. Verified by
      `git ls-files | grep -c playwright` reporting 0.
- [x] 1.4 Run `bash scripts/verify.sh`. Verified by "verify: all checks passed".

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, and watch
      the three hosted jobs: `ci`, `deploy`, `smoke`. Verified by `gh run watch`
      reporting `success` and by the smoke step printing the URL it proved.
