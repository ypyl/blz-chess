# Tasks

## 1. Scaffold the C# solution

- [ ] 1.1 Create `BlzChess.sln`, `src/BlzChess.Core`, `src/BlzChess.Web` (from
      `dotnet new blazorwasm --pwa`), `tests/BlzChess.Core.Tests` (from
      `dotnet new xunit`). Verified by `dotnet new sln` + `dotnet sln add` for
      all three and `ls` showing every project file.
- [ ] 1.2 Add `global.json` pinned to the 10.0 band, `Directory.Build.props`
      with `Nullable`, `TreatWarningsAsErrors`, and
      `RestorePackagesWithLockFile`. Verified by `dotnet build -warnaserror`
      succeeding and `packages.lock.json` existing after restore.
- [ ] 1.3 Set `<base href="/blz-chess/" />` in `wwwroot/index.html`, add
      `.nojekyll`, and disable precompressed publish output. Verified by
      `dotnet publish -c Release` and grepping the base path and `.nojekyll` in
      the output folder.
- [ ] 1.4 Add `.editorconfig` and the `bin/`, `obj/`, `publish/` ignore rules.
      Verified by `git status --short` staying clean after a build.
- [ ] 1.5 Add a `Core` placeholder with one xUnit test that names the rule it
      will guard, so `dotnet test` runs one real case. Verified by `dotnet test`
      reporting one passing test.

## 2. Pipeline

- [ ] 2.1 `.github/workflows/ci.yml`: restore locked, format check, build with
      warnings as errors, test. Verified by reading the workflow against
      `dotnet` command order and by the local run of the same four commands.
- [ ] 2.2 `.github/workflows/deploy.yml`: `main` only, publish, copy
      `index.html` to `404.html`, upload and deploy Pages, with `pages: write`
      and `id-token: write` scoped to it. Verified by reading permissions and by
      the first green run once a remote exists.
- [ ] 2.3 Fill the CI block in `AGENTS.md` with the `gh` commands that trigger,
      wait, report status, and read logs. Verified by
      `bash scripts/check-change.sh` reporting the block complete, and by
      running the commands once a remote exists.
- [ ] 2.4 Leave the Deploy block empty and record why. Verified by
      `scripts/check-change.sh` reporting "no Deploy declared".

## 3. Contract documents

- [ ] 3.1 Replace `README.md` with the product README: what it is, who it is
      for, what it is not, how to run it, where to start. Verified by
      `scripts/check-change.sh` no longer reporting the template README.
- [ ] 3.2 Fill **What this is** and every Project facts line in `AGENTS.md`.
      Verified by `scripts/check-change.sh` reporting Project facts filled in.
- [ ] 3.3 Write the first `CONVENTIONS.md` rules: formatter, error handling,
      test layout, dependency policy, lockfile policy. Verified by reading the
      four sections with no "None yet" left.
- [ ] 3.4 Add `ARCHITECTURE.md` with C4 Context and Container views: browser app,
      GitHub Pages, Actions. Verified by `architecture-doc` skill output and one
      Mermaid block per view.
- [ ] 3.5 Add the `## Unreleased` entry in `CHANGELOG.md` and the ADR row in
      `adr/README.md`. Verified by `grep` for the entry and the row.
- [ ] 3.6 Add `LICENSE` with GPLv3 and name it in `README.md`, because the engine
      the ADR chooses is GPLv3. Verified by `head -3 LICENSE` and a `grep` for
      the licence line in `README.md`.

## 4. Engine groundwork

- [ ] 4.1 Record the engine shape in `ARCHITECTURE.md` as a container-adjacent
      element: the wasm worker loads in the browser, no new external system.
      Verified by the diagram naming the worker and the vendored asset.
- [ ] 4.2 Record the vendoring rule in `CONVENTIONS.md`: engine version pinned in
      one place, file sha256 beside it, no CDN, lazy load. Verified by the rule
      appearing with a trigger.
- [ ] 4.3 Wire nothing. No engine code lands in this change; the first feature
      change that adds an opponent does it against this record. Verified by
      `grep -r stockfish src/ test/` returning no hits.

## 5. Verify and land

- [ ] 5.1 `bash scripts/verify.sh` green: invariant checks plus the declared
      lint, build, and test commands. Verified by the reported exit status.
- [ ] 5.2 `node scripts/gate.mjs bootstrap --verify` matches every recorded
      hash. Verified by exit 0 after the approval block is written.
- [ ] 5.3 Run `review-change`. Verified by its findings being resolved or
      recorded.
- [ ] 5.4 Archive the change, commit `feat` and `chore: archive bootstrap`, merge
      to `main` with `--no-ff`. Verified by `git log --oneline` on `main` showing
      the three commits and `openspec/changes/archive/` holding the folder.
