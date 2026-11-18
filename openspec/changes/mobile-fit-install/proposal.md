# The app fits a phone and installs as a PWA

L0

## Why

Measured on the live site at phone sizes, two things were wrong.

The phone layout was dead. A stray double brace in the stylesheet, left by an
earlier splice, stopped every rule after it from applying, including the whole
media block that gives the status its own row. The header has been one squeezed
row since that change shipped, and nobody noticed because portrait phones still
fit.

A landscape phone scrolled vertically: the board's width was capped only by the
width of the screen, so 844x390 produced 544 pixels of board and 272 pixels of
overflow.

The install half of the request was already true and unguarded: the manifest has
the name, the standalone display, the start URL, the app's colours, and the 192,
512, and SVG icons an install prompt asks for, served with the right MIME type,
with a fetch-handling service worker behind them. Nothing checked it.

## What Changes

The stray brace goes, so the phone header is two rows as designed. The board and
its loading twin are additionally capped by the viewport height in dynamic units
(the plain `vh` stays as the fallback for older browsers), so every orientation
holds the whole board without scrolling.

`scripts/verify.sh` gains the installability check: it parses the manifest, asserts
the fields and icon sizes an install prompt requires, asserts each icon file exists,
and asserts the page links both the manifest and the apple-touch icon. The deploy
smoke also fetches the 512 icon, which it did not before.

## Routing

0. **Observable behavior changes?** The phone header wraps as designed and landscape
   stops scrolling; nothing a spec promises moves.
1. **Undoable in one commit?** Yes: one brace, three width rules, two checks.
2. **Can a machine prove it correct?** Yes: measured, `scrollHeight` equals
   `innerHeight` at 360x740, 390x844, 412x915, and both landscapes, and the loading
   view holds at landscape too; the installability check fails on any missing piece.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: a layout defect and a machine check, no behavior a spec owns.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/css/app.css`, `scripts/verify.sh`,
`.github/workflows/deploy.yml`, `DESIGN.md`, and `CHANGELOG.md`.
