# Make the loading screen look like the app

L0

## Why

The page shows a generic two-circle spinner with "Loading" while the WebAssembly
runtime arrives, which is the template's own device. It says nothing about what is
loading, and on a slow connection that screen is the whole product for several
seconds.

## What Changes

The spinner becomes the app's mark over one rank of the board. The rank fills left to
right in the accent colour as the runtime arrives, the percentage sits underneath,
and the mark bobs a few pixels until it stops. Nothing new is fetched: the mark is
the icon that already ships, and the pieces are preloaded.

Blazor publishes its progress as `--blazor-load-percentage` and
`--blazor-load-percentage-text` on the document root, so the rank and the number are
plain CSS off those two values and no script of ours is involved.

The markup stays in `index.html`, where Blazor replaces it on boot, and the styles
replace the template's spinner rules, including the reduced-motion case.

## Routing

0. **Observable behavior changes?** The wait looks different. No spec requirement
   moves, and nothing a player can do changes.
1. **Undoable in one commit?** Yes: one markup block and one CSS block.
2. **A machine can prove it correct?** Yes: with framework requests delayed, the
   document reports a real percentage and the page renders the rank, the fill, the
   mark, and the number in step with it.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: presentation only, fully reversible.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/index.html`, `src/BlzChess.Web/wwwroot/css/app.css`, and the
loading section in `DESIGN.md`.
