# Tasks

All checked at the end of the change. Deviations are noted under the task they
belong to.

## 1. Scaffold the C# solution

- [x] 1.1 Create the solution, `src/BlzChess.Core`, `src/BlzChess.Web` (from
      `dotnet new blazorwasm --pwa`), `tests/BlzChess.Core.Tests` (from
      `dotnet new xunit`), all added to the solution. Done: the .NET 10 CLI writes
      `BlzChess.slnx`, not `BlzChess.sln`.
- [x] 1.2 Add `global.json` pinned to the 10.0 band, `Directory.Build.props` with
      `Nullable`, `TreatWarningsAsErrors`, and `RestorePackagesWithLockFile`.
      Done: `dotnet build -warnaserror` succeeds, and one `packages.lock.json`
      per project is committed.
- [x] 1.3 Configure the published shell for GitHub Pages: `.nojekyll` in
      `wwwroot`, and `EnableDefaultCompressionFormats=false` so Pages does not
      get precompressed files it cannot serve. Done, and the base path is set by
      the deploy workflow instead of the source file, so `dotnet run` keeps
      working at the server root.
- [x] 1.4 Add `.editorconfig` and the `bin/`, `obj/`, `dist/` ignore rules. Done:
      `git status --short` stays clean after a build.
- [x] 1.5 Add a `Core` placeholder with one real xUnit test. Done: `Fen` and
      `FenTests`, one passing test.

## 2. Pipeline

- [x] 2.1 `.github/workflows/ci.yml`: restore locked, format check, build with
      warnings as errors, test. Done, with the same four commands run locally.
- [x] 2.2 `.github/workflows/deploy.yml`: `main` only, publish, rewrite the base
      path, copy `index.html` to `404.html`, upload and deploy Pages, with
      `pages: write` and `id-token: write` scoped to it. Done. Not yet observed
      green: the remote does not exist.
- [x] 2.3 Fill the CI block in `AGENTS.md`. Done, and
      `scripts/check-change.sh` reports it complete.
- [x] 2.4 Leave the Deploy block empty and record why. Done: "no Deploy declared".

## 3. Contract documents

- [x] 3.1 Replace `README.md` with the product README. Done. The check script no
      longer sees the template README.
- [x] 3.2 Fill **What this is** and every Project facts line in `AGENTS.md`.
      Done, and the check script reports Project facts filled in.
- [x] 3.3 Write the first `CONVENTIONS.md` rules. Done: formatting, error
      handling, testing, dependencies.
- [x] 3.4 Add `ARCHITECTURE.md` with C4 Context and Container views. Done, with
      Mermaid `flowchart` blocks and a container table naming paths.
- [x] 3.5 Add the `## Unreleased` entry in `CHANGELOG.md` and the ADR row in
      `adr/README.md`. Done.
- [x] 3.6 Add `LICENSE` with GPLv3 and name it in `README.md`. Done.

## 4. Engine groundwork

- [x] 4.1 Record the engine in `ARCHITECTURE.md` as a container that loads in the
      browser. Done, and marked as not yet shipped.
- [x] 4.2 Record the vendoring rule in `CONVENTIONS.md`: version pinned in one
      place, sha256 beside it, no CDN, lazy load. Done.
- [x] 4.3 Wire nothing. Done: `grep -r stockfish src/ tests/` finds no code.

## 5. Verify and land

- [x] 5.1 `bash scripts/verify.sh` green. Done: invariants, 20 gate tests, lint,
      build, and test all passed.
- [x] 5.2 `node scripts/gate.mjs bootstrap --verify` matches every recorded hash.
      Done: exit 0.
- [x] 5.3 `review-change` run. Done; findings and their fixes are in the archive
      commit message.
- [ ] 5.4 Archive the change, commit `feat` and `chore: archive bootstrap`, merge
      to `main` with `--no-ff`.
- [ ] 5.5 The `wasm-tools` workload is not installed on this machine, so local
      publish is untrimmed (18 MB) while CI trims. Needs the human's permission
      for one machine-wide install: `dotnet workload install wasm-tools`.
