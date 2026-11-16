# Sync the process scripts with the template

L0

## Why

Four process files in this repository came from an older copy of the template:
the gate page does not default to the gate its level calls for, its markdown
renderer breaks a wrapped list item across elements, and two skills still tell an
agent to run the gate bare. Nothing product-facing is wrong. The process copy is
stale, and the template has since moved.

## What Changes

Replace four files with the template's current versions, byte for byte:
`scripts/gate.mjs`, `scripts/gate.test.mjs`, `.agents/skills/bootstrap/SKILL.md`,
`.agents/skills/change-level/SKILL.md`. No product behavior, no contract document,
no dependency, no durable spec.

## Routing

0. **Observable behavior changes?** No. The gate page and two skills are process,
   not product, and no requirement in `openspec/specs` moves.
1. **Undoable in one commit?** Yes. Four files, no data, no published contract.
2. **A machine can prove it correct?** Yes. `node --test scripts/gate.test.mjs`
   covers the renderer, the gate selection, the hashes, and the approval record,
   and `scripts/check-change.sh` re-verifies the bootstrap approval against the
   new hashing code.
3. **Does anything outside depend on it?** No. The scripts are used by this
   repository only, and `AGENTS.md` does not name their internals.

Level **L0**, `skip_specs: true`. No gate: a machine proves it, and the human
approved the sync before it started.

## Capabilities

None. No requirement is added, changed, or left unserved.

## Impact

Four files under `scripts/` and `.agents/skills/`. The bootstrap approval recorded
in `openspec/changes/archive/2026-11-16-bootstrap/proposal.md` still verifies,
because `artifactHash`, `stripRecord`, and `verify` are untouched by this sync.
