# Stop .gitignore from swallowing the site's own assets

L0

## Why

The deployed page is stuck on its loading screen. The browser console shows
`lib/bootstrap/dist/css/bootstrap.min.css` returning 404, and it is not alone:
44 files under `src/BlzChess.Web/wwwroot` never reached the repository.

The cause is one unanchored pattern. The bootstrap change added `dist/` to
`.gitignore` for the publish output at the repository root, and that pattern also
matches `wwwroot/lib/bootstrap/dist/`. Git ignored the asset folder silently, the
build stayed green because the files were on the dev machine, and the defect only
appeared once the site was served from a machine that only has what was committed.

## What Changes

The publish pattern becomes `/dist/`, anchored to the repository root, and the 44
assets join the repository.

`scripts/verify.sh` gains the signal that was missing: it fails when any file
under `src/BlzChess.Web/wwwroot` is gitignored, naming the files. A build cannot
see this class of bug, because the machine that builds usually has the file.

## Routing

0. **Observable behavior changes?** The deployed page goes from broken to the
   shell it was always meant to serve. No requirement is added or altered; no
   spec exists to move.
1. **Undoable in one commit?** Yes, one pattern and one script block.
2. **A machine can prove it correct?** Yes.
   `git ls-files -o -i --exclude-standard src/BlzChess.Web/wwwroot` listed 44
   files before the fix and none after, and `scripts/verify.sh` now runs that
   comparison on every invocation.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0** by `fix-bug`: a bug that restores intended behavior, reversible,
provable, unshared.

## Capabilities

None. No requirement moves.

## Impact

`.gitignore`, `scripts/verify.sh`, and the 44 bootstrap assets. The deployed site
changes from a blank loading screen to the shell.
