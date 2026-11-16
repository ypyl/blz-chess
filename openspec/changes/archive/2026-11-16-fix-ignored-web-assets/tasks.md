# Tasks

## 1. Signal, then fix

- [x] 1.1 Capture the failing signal before touching anything:
      `git ls-files -o -i --exclude-standard src/BlzChess.Web/wwwroot` lists 44
      files. Done.
- [x] 1.2 Anchor the publish pattern to the repository root. Verified by the same
      command listing 0 files.
- [x] 1.3 Add that command to `scripts/verify.sh` as a project check, naming the
      offending files. Verified by `bash scripts/verify.sh` reporting "every file
      under wwwroot is tracked".
- [x] 1.4 Commit the 44 assets. Verified by `git ls-files src/BlzChess.Web/wwwroot`
      counting 53 files.
- [x] 1.5 Run `bash scripts/verify.sh`. Verified by "verify: all checks passed".

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, and watch
      both hosted runs. Verified by `gh run watch` reporting `success`.
- [ ] 2.2 Smoke the deployed URL in a browser: the page renders its heading, the
      console reports no errors, and a deep link lands on the app shell. Verified
      by the browser snapshot and console log.
