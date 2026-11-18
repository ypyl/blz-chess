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

# Installability, as a browser sees it: a manifest with the fields and icon sizes an
# install prompt asks for, the icon files really present, and the page linking both
# the manifest and the touch icon. The app has no install server-side; this is the
# whole contract.
if command -v node >/dev/null 2>&1; then
  installability=$(node -e '
const fs = require("fs");
const missing = [];
const manifest = JSON.parse(fs.readFileSync("src/BlzChess.Web/wwwroot/manifest.webmanifest", "utf8"));
if (!manifest.name) missing.push("manifest has no name");
if (!manifest.start_url) missing.push("manifest has no start_url");
if (manifest.display !== "standalone") missing.push("display is not standalone");
const sizes = manifest.icons.map((i) => i.sizes).join(" ");
if (!sizes.includes("192") || !sizes.includes("512")) missing.push("manifest icons lack a 192 and a 512");
for (const icon of manifest.icons) {
  if (!fs.existsSync("src/BlzChess.Web/wwwroot/" + icon.src)) missing.push("missing icon " + icon.src);
}
const index = fs.readFileSync("src/BlzChess.Web/wwwroot/index.html", "utf8");
if (!index.includes("manifest.webmanifest")) missing.push("the page does not link the manifest");
if (!index.includes("apple-touch-icon")) missing.push("the page links no apple-touch-icon");
if (missing.length > 0) {
  for (const line of missing) console.log(line);
  process.exit(1);
}
console.log("manifest, icons, and links all present");
')
  if [[ $? -ne 0 ]]; then
    printf '
==> Installability: the browser would refuse to install
'
    printf '%s
' "$installability" | sed 's/^/  FAIL /'
    status=1
  else
    printf '
==> Installability: %s
' "$installability"
  fi
fi

# The loading board is static markup, so nothing derives its colours the way the game
# board does. Assert the real board's parity here: a square is light when its file and
# rank differ in parity, which makes a8 light and a1 dark. An inverted loading board
# shipped once, and it is invisible until someone looks at it.
if command -v node >/dev/null 2>&1; then
  parity=$(node -e '
const fs = require("fs");
const html = fs.readFileSync("src/BlzChess.Web/wwwroot/index.html", "utf8");
const start = html.indexOf("<div class=\"loading-board\"");
const block = start < 0 ? "" : html.slice(start, html.indexOf("</div>", start));
const cells = [...block.matchAll(/class="(light|dark)"/g)].map((m) => m[1]);
if (cells.length !== 64) {
  console.log("expected 64 squares, found " + cells.length);
  process.exit(1);
}
const expected = (i) => (((7 - Math.floor(i / 8)) + (i % 8)) % 2 === 1 ? "light" : "dark");
const wrong = cells.findIndex((cell, i) => cell !== expected(i));
if (wrong >= 0) {
  console.log("square " + wrong + " is " + cells[wrong] + ", a real board has " + expected(wrong));
  process.exit(1);
}
console.log("64 squares, a8 light and a1 dark");
')
  if [[ $? -ne 0 ]]; then
    printf '
==> Loading board: colours inverted
'
    printf '%s
' "$parity" | sed 's/^/  FAIL /'
    status=1
  else
    printf '
==> Loading board: %s
' "$parity"
  fi
fi

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
