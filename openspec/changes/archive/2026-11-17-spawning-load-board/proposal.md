# Fill the board while the app loads

L0

## Why

The loading screen shows the app's mark over a single rank that wipes left to right.
It reads as a progress bar with chess decoration. A full board that fills square by
square says the same thing in the product's own vocabulary, and it is the screen a
player sees most often on a slow connection.

## What Changes

The mark and the rank are replaced by an eight-by-eight board of the app's cream and
green, filling square by square in two shades of the accent as the runtime arrives.
The percentage stays underneath. The filled half keeps the board's alternation, so
progress reads as a board being played into, not as a block of colour.

Blazor publishes progress as a custom property on the document, so a short inline
loop reads it, fills that share of the sixty-four squares, and stops as soon as
Blazor replaces the loading markup. Nothing is fetched for this beyond the CSS.

## Routing

0. **Observable behavior changes?** Only what the wait looks like. No spec
   requirement moves.
1. **Undoable in one commit?** Yes: one markup block, one inline loop, one CSS block.
2. **A machine can prove it correct?** Yes: with the runtime delayed, the page read
   the published percentage and filled exactly that share of the squares (one square
   at 1.46%, thirty at 47%), each filled square carrying the accent and the rest the
   board's colours.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: presentation only, fully reversible.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/index.html`, `src/BlzChess.Web/wwwroot/css/app.css`, and the
loading section of `DESIGN.md`.
