# Prove the deploy serves, and drop the template's leftovers

L0

## Why

A green `deploy` run means GitHub accepted the artifact. It does not mean the site
works: earlier today the pipeline went green on a page that never left its loading
screen, and only a browser found it. The pipeline needs to say one thing about the
deployed site, and it needs to say it in the run, not in someone's memory.

Two leftovers from the template also shipped: an `About` link in the layout that
points at Microsoft's Blazor docs, and a browser-check file that the
`.playwright-cli` tool wrote into the repository root, which the previous commit
swept in with `git add -A`.

## What Changes

- The deploy workflow gains a `smoke` job that runs after `deploy`: it fetches the
  site, retrying while Pages propagates, proves the base path is `/blz-chess/`,
  extracts the boot script name from the shell and fetches it, and fetches the
  manifest. A missing asset now fails the run.
- The `About` link goes, so the shell carries no text that belongs to the
  template instead of the product.
- `.playwright-cli/` is ignored and its committed file is removed. Tool scratch
  output belongs in `.tmp/`, and this is the safety net for when it is not.

## Routing

0. **Observable behavior changes?** The deployed page loses one link that pointed
   to documentation for a framework, not to the product. No requirement changes.
1. **Undoable in one commit?** Yes: one job, one link, one ignore rule.
2. **A machine can prove it correct?** Yes, and the proof is the change: the new
   job fails when the deployed shell or its boot script is missing. It was
   written against the failure that actually happened.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.
   The CI block in `AGENTS.md` names `ci.yml`, not this job.

Level **L0**, `skip_specs: true`. No gate.

## Capabilities

None. No requirement moves.

## Impact

`.github/workflows/deploy.yml`, `src/BlzChess.Web/Layout/MainLayout.razor`,
`.gitignore`, and the removal of `.playwright-cli/` from the repository.
