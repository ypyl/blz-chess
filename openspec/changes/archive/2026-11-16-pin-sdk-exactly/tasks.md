# Tasks

## 1. Pin

- [x] 1.1 Set `rollForward: disable` on SDK 10.0.402. Verified by
      `dotnet --version` reporting 10.0.402.
- [x] 1.2 Correct the Toolchain fact. Verified by reading it.
- [x] 1.3 Run the CI steps locally. Verified by `dotnet restore --locked-mode`,
      `dotnet format --verify-no-changes`, `dotnet build -warnaserror`, and
      `dotnet test` all exiting 0.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, and watch
      both hosted runs. Verified by `gh run watch` reporting `success` for `ci`
      and for `deploy`.
