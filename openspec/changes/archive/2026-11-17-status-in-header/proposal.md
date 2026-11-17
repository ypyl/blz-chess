# Put the result in the header so the board never moves

L0

## Why

The status line sat under the board, and the result added a second "New game"
button below it. When a game ended the text grew and a button appeared, so the
board was pushed up the page and the whole view jumped at the exact moment the
player was looking at it.

Moving the message does not help while it lives outside the header: any element
that appears or grows below the board moves the board.

## What Changes

The header becomes level name, status, button, in that order, and the status is a
slot that exists in every state: "Your move", "Thinking…", and the result all take
the same room. The button is always there, so a finished game adds nothing.

Below the board there is nothing left but an error message, which appears only when
the engine fails.

The middle column absorbs the differences: the level name and the button keep their
size and the message is the part that clips, with the full text in `title`. On a
phone the message gets its own row, again in every state, so the geometry is the
same before and after the result.

## Routing

0. **Observable behavior changes?** The page states the same result; where it is
   stated changes. No spec requirement moves.
1. **Undoable in one commit?** Yes: one component and one stylesheet.
2. **A machine can prove it correct?** Yes, and it was measured: the board's top
   edge is `107.2` at desktop width and `262.2` at phone width for all five result
   messages, with no clipping.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: a layout defect, measured before and after.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/Pages/Play.razor`, `src/BlzChess.Web/wwwroot/css/app.css`, and the
layout line in `DESIGN.md`. The unused `.primary` rule goes.
