# Trunk is master, not main

L0

## Why

The human asked for it. The repository was created with `git init -b main`, and the trunk
branch is the one thing every change merges into, so the name is worth having the one
they asked for.

## What Changes

The trunk branch is renamed to `master` on GitHub, and the two places that name it follow:
the deploy workflow's push filter, which is the branch that publishes the site, and the
Project facts in this file.

`ci` runs on every push and pull request and names no branch, so it needs no change. The
rename carries the history, the default branch, and the Pages source with it; nothing is
rebuilt or re-uploaded.

## Routing

0. **Observable behavior changes?** No. The site, the workflows' contents, and every
   command in Project facts are the same; only the branch a push targets changes.
1. **Can it be undone easily?** Yes: rename back, and revert one commit that carries the
   two names.
2. **Can a machine prove it correct?** Yes, by the pipelines themselves: `gh repo view`
   reports the default branch, and the change merges onto `master` so `ci` and `deploy`
   run from it and report green.
3. **Does anything outside depend on it?** The `deploy` filter and Project facts, both
   updated here. No published contract, no consumer.

Level **L0**: configuration and a fact, reversible in one commit, proven by a green run.

## Notes

The rename itself is repo maintenance, not a working commit on trunk, so it takes no
branch of its own. Every commit in this change still lands through `change/trunk-master`
with `git merge --no-ff`.
