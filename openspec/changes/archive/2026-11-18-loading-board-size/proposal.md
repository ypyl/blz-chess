# Load in the board the game will use

L0

## Why

The loading board was a smaller board of its own, twenty rem wide and centred on the
screen, while the game board is the shell's width, up to thirty-four rem, and sits
under a header. So the first thing a player saw was a small board that then grew and
moved as the app took over.

## What Changes

The loading view is built in the game's own layout: the same `.shell`, a header row
holding invisible placeholders whose sizes match the level control and the action,
the same `.board` geometry with no gap, and the percentage in the status slot, where
"Your move" appears a moment later. The inline loop and the pieces stay as they are.

Measured: the loading board is `544x544` at `left 368, top 107.2` on a desktop
viewport and `358.8` wide at `top 261.8` on a phone, which is exactly where and what
size the game board is at both widths.

## Routing

0. **Observable behavior changes?** Only the loading screen. No spec requirement
   moves.
1. **Undoable in one commit?** Yes: one markup block and one CSS block.
2. **A machine can prove it correct?** Yes: the board's box is measured in both
   states at both widths and matches to the pixel.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: presentation only, fully reversible.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/index.html`, `src/BlzChess.Web/wwwroot/css/app.css`, and the
loading section of `DESIGN.md`.
