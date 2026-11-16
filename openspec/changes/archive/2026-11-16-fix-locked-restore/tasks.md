# Tasks

## 1. Pin and prove

- [x] 1.1 Pin the SDK in `global.json` and install it from that file in both
      workflows. Verified by `dotnet --version` reporting 10.0.402 locally and by
      the runner's setup step naming 10.0.402.
- [x] 1.2 Regenerate the lock files against the pinned SDK. Verified by
      `dotnet restore --force-evaluate` leaving every `packages.lock.json`
      unchanged.
- [x] 1.3 Run the CI steps locally in order. Verified by
      `dotnet restore --locked-mode` then `dotnet format --verify-no-changes`,
      `dotnet build -warnaserror`, and `dotnet test` all exiting 0.
- [x] 1.4 Record the reason in the Toolchain fact so the next bump is a decision
      and not a surprise. Verified by reading the fact.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, and watch
      both hosted runs. Verified by `gh run watch` reporting `success` for `ci`
      and for the `deploy` job.
