#!/usr/bin/env bash
# Runs the whole machine check in one place: the invariants in
# scripts/check-change.sh, then the lint, build, and test commands declared in
# Project facts in AGENTS.md.
#
#   bash scripts/verify.sh
#
# Add a project check here, not in the pre-commit hook. The hook runs only the
# fast half, so a commit stays cheap; this script is what a release candidate
# and a hosted runner execute.
#
# Exit status: 0 when everything passed, 1 when anything failed. A step that
# Project facts does not declare is skipped and reported as skipped.

set -uo pipefail

root=$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)
cd "$root" || exit 1

status=0

fact() {
  sed -n "s/^- $1:[[:space:]]*//p" AGENTS.md 2>/dev/null | head -1
}

run() {
  local label=$1 cmd=$2
  printf '\n==> %s: %s\n' "$label" "$cmd"
  bash -c "$cmd" || { printf '  FAIL %s\n' "$label"; status=1; }
}

bash scripts/check-change.sh || status=1

if command -v node >/dev/null 2>&1 && [[ -f scripts/gate.test.mjs ]]; then
  run "Gate page" "node --test scripts/gate.test.mjs"
else
  printf '
==> Gate page: node or the test file is missing, skipped
'
fi

# --- project checks --------------------------------------------------------

# GitHub Pages does not serve dotfiles, so a dotfile in the site is a 404 in the
# service worker's asset list, and one failed asset fails the whole install: the
# app stops being installable and offline-capable.
dotfiles=$(find src/BlzChess.Web/wwwroot -name '.*' -type f 2>/dev/null)
if [[ -n "$dotfiles" ]]; then
  printf '
==> Web assets: dotfiles in the site
'
  printf '%s
' "$dotfiles" | sed 's/^/  FAIL /'
  status=1
else
  printf '
==> Web assets: no dotfiles the host will not serve
'
fi

# A file the site needs that .gitignore silently swallows never reaches the
# published site: the build is green, the page 404s in the browser. This check
# fails on the file, not on the symptom.
ignored_assets=$(git ls-files -o -i --exclude-standard src/BlzChess.Web/wwwroot 2>/dev/null)
if [[ -n "$ignored_assets" ]]; then
  printf '
==> Web assets: .gitignore swallows files the site needs
'
  printf '%s
' "$ignored_assets" | sed 's/^/  FAIL /'
  status=1
else
  printf '
==> Web assets: every file under wwwroot is tracked
'
fi

for step in Lint Build Test; do
  cmd=$(fact "$step")
  if [[ -z "$cmd" ]]; then
    printf '\n==> %s: not declared in Project facts, skipped\n' "$step"
  else
    run "$step" "$cmd"
  fi
done

printf '\n'
if [[ $status -eq 0 ]]; then
  printf 'verify: all checks passed\n'
else
  printf 'verify: failures above\n'
fi
exit $status
