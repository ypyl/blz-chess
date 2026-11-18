# The status under the board, not in the header

L0

## Why

"Your move." sat between the level and the action buttons, which is a lot of
header for two words, and a player reads the board, not the header. The status
belongs under the board, where the eye already is.

## What Changes

The status moves below the board, centered, and the header holds the level and the
actions only. The loading view mirrors the same shape, so loading and game agree
again.

The old jump trap is why the slot has a reserved height: the shell is centred
vertically, so a line below the board that appears or grows shifts the board above
it. Every status - "Your move", "Thinking…", any result - is one line in the same
reserved room, and the board was measured at the same position for all of them.

Landscape phones needed the board's height cap widened by the slot: without it,
844x390 scrolled by 14 pixels. With the wider cap every tested size holds the whole
page: 360x740, 390x844, 412x915, both landscapes, and the desktop board is
untouched at 544.

## Routing

0. **Observable behavior changes?** Where the same words are shown. No spec
   requirement moves.
1. **Undoable in one commit?** Yes: one component block, one stylesheet block, one
   loading-markup block.
2. **Can a machine prove it correct?** Yes: the board's top edge is `87.2` in every
   status at desktop width, and `scrollHeight` equals `innerHeight` at every tested
   viewport, loading view included.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: presentation only, fully reversible.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/Pages/Play.razor`, `src/BlzChess.Web/wwwroot/css/app.css`,
`src/BlzChess.Web/wwwroot/index.html` (the loading view), `DESIGN.md`,
`CHANGELOG.md`.
