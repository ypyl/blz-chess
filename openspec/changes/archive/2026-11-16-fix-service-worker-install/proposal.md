# Make the service worker installable

L0

## Why

The deployed app never installs its service worker. Registering it reaches
`installing` and then `redundant`: the published worker caches every asset the
build lists, and that list contains `.nojekyll`. GitHub Pages does not serve
dotfiles, so the file answers 404, `cache.addAll` rejects, and the install fails.

Nothing said so earlier. Pages accepted the deployment, the smoke job fetched the
shell and the engine, the game plays, and the console stays quiet. The app simply
never becomes installable or offline-capable, which `adr/0001` and the README both
promise.

The file is not needed either: `.nojekyll` exists to stop Jekyll from processing a
site that Pages builds from a branch. This site is deployed from an artifact by
`actions/deploy-pages`, which never passes through Jekyll.

## What Changes

`.nojekyll` leaves `wwwroot`, since the host it was there for is not the host in
use.

Two checks learn about the class instead of the instance:

- `scripts/verify.sh` fails when any file under `wwwroot` starts with a dot, naming
  the files, because the host will 404 it and the service worker will not install.
- The deploy workflow refuses to publish a site containing dotfiles, so a red
  deploy catches it even when the local check was skipped.

## Routing

0. **Observable behavior changes?** The app gains the offline install its README
   already claims. No spec requirement moves: `openspec/specs/game/spec.md` says
   nothing about installability, and this restores what the ADR decided.
1. **Undoable in one commit?** Yes: one deleted marker file and two checks.
2. **A machine can prove it correct?** Yes, in both directions. The failing signal
   is the worker's state going to `redundant`; after the fix it reaches
   `activated`, and the new checks fail when a dotfile returns.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0** by `fix-bug`: reversible, provable, unshared.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/.nojekyll` is deleted, `scripts/verify.sh` gains a check,
and `.github/workflows/deploy.yml` gains a step. Nothing else changes.
