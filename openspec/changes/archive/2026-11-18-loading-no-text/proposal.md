# The pieces are the loading indicator, and nothing else is

L0

## Why

The loading view showed "Loading 34%" under the board, which is a number a spinner
would carry. It breaks the picture: a board being set up has no caption, and the
percentage adds text the player never asked to read. The pieces arriving one by one
already are the progress.

## What Changes

The text goes. The status slot becomes a third invisible placeholder, exactly the
size "Your move" will occupy, so the header keeps its height and the board still
sits where the game board sits. The percentage custom property stays in use by the
inline loop, which is what decides how many pieces are placed; it is just no longer
printed.

Nothing else moves: same shell, same board, same piece order.

## Routing

0. **Observable behavior changes?** Only the loading view loses its text. No spec
   requirement moves.
1. **Undoable in one commit?** Yes: one element, one CSS rule.
2. **A machine can prove it correct?** Yes: with the runtime stalled the page's
   visible text is empty while eighteen of thirty-two pieces are placed, and the
   board's box is unchanged.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: presentation only, fully reversible.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/index.html`, `src/BlzChess.Web/wwwroot/css/app.css`, and the
loading section of `DESIGN.md`.
