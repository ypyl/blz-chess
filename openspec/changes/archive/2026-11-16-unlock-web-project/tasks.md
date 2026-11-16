# Tasks

## 1. Unlock and document

- [x] 1.1 Set `RestorePackagesWithLockFile=false` on the web project, delete its
      lock file, and mark the deferred lock with a `shortcut:` and a trigger.
      Verified by `grep` for the property and the marker in the csproj.
- [x] 1.2 Keep the lock files for `BlzChess.Core` and the tests. Verified by
      `git ls-files '*packages.lock.json'` listing exactly two.
- [x] 1.3 Update `CONVENTIONS.md` and the Toolchain fact. Verified by reading
      both.
- [x] 1.4 Run the CI steps locally. Verified by `dotnet restore --locked-mode`,
      `dotnet format --verify-no-changes`, `dotnet build -warnaserror`, and
      `dotnet test` all exiting 0.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, and watch
      both hosted runs. Verified by `gh run watch` reporting `success` for `ci`
      and for `deploy`, and by the deployed URL answering.
