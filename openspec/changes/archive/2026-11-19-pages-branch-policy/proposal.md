# Let Pages deploy from master

L0

## Why

Renaming trunk to `master` left the site unable to publish. The `github-pages` environment
carries a branch policy, and that policy allowed exactly one branch: `main`. The deploy
job did not fail at a step; it failed before the first one, in two seconds with no steps,
which is a runner refusing a ref the environment does not permit.

Nothing in the repository said so. The workflow publishes on pushes to `master`, the build
passed, and only the deploy was refused, so the fault was in the repository's settings and
nowhere in the files.

## What Changes

The environment's branch policy allows `master` instead of `main`. It was the only entry,
so the two cannot overlap: the old one is deleted rather than kept beside the new one.

## Routing

0. **Observable behavior changes?** No: the site is the same site, deployed from the same
   commit, by the same job.
1. **Can it be undone easily?** Yes: one API call each way.
2. **Can a machine prove it correct?** Yes: a deploy run on `master` reaches `smoke` and
   reports green, where before it failed before the first step.
3. **Does anything outside depend on it?** No file, no contract: the policy lives in
   GitHub, not in the repository, which is exactly why it was missed.

Level **L0**: a setting, reversible, and proven by the pipeline it unblocks.

## Note

The setting cannot be described in a file, so this change's only artifacts are this
proposal and its tasks. The evidence is the run.
