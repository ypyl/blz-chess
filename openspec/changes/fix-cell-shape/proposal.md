# Make every cell square, and take the lines off the board

L0

## Why

The board's grid had a column template and no row template. Rows were therefore
sized by their contents: a row holding pieces grew to fit them, and an empty row
collapsed to the gap between tracks. The cells were rectangles of two different
heights, and the board read as a banded pattern rather than eight equal ranks.

The one-pixel separator added in the last change was a workaround for the empty
half looking flat. With the rows fixed, the separator is noise: two colours divide
a chess board, and a dark line between every square makes it look tiled.

## What Changes

Both grid axes are explicit, `repeat(8, minmax(0, 1fr))`, so a piece can neither
widen nor heighten its track. Squares get `min-height: 0`, images fill their square
with `object-fit: contain`, and the separator gap goes, so the board is one
continuous surface of alternating colours.

`DESIGN.md` records the result instead of the workaround.

## Routing

0. **Observable behavior changes?** The board is square and plain. No spec
   requirement moves.
1. **Undoable in one commit?** Yes: one CSS file and one doc line.
2. **A machine can prove it correct?** Yes, and it was measured: every square is
   `68.0x68.0` and there is one distinct row height at the tested width.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: a layout defect with a measurement that shows it gone.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/css/app.css` and `DESIGN.md`.
