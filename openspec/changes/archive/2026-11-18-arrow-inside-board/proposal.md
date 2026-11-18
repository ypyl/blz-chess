# Put the arrow inside the board

L0

## Why

The arrow existed and pointed at the right squares in its own coordinates, but it was
drawn in the middle of the window instead of on the board. The `<svg>` was a sibling
of the board, so `position: absolute; inset: 0` resolved against the nearest
positioned ancestor, which was none of them, and it spanned the whole page.

The previous change verified the arrow's `x1/y1/x2/y2` attributes, which were
correct, and never checked where the arrow landed on screen. That is the same class
of mistake as trusting a build over a browser: the data was right, the picture was
not.

## What Changes

The overlay becomes a child of the board, so its 800-by-800 view box maps onto the
board's box. `preserveAspectRatio="none"` keeps that mapping exact when the board is
not exactly square to the pixel, and a comment in the markup says why the placement
matters.

## Routing

0. **Observable behavior changes?** The arrow moves onto the board. No spec
   requirement moves.
1. **Undoable in one commit?** Yes: one markup move and one attribute.
2. **A machine can prove it correct?** Yes, by screen geometry rather than attributes:
   the overlay's box equals the board's box to within a pixel at 390x844, 844x390,
   and 1280x800, the shaft's start point lands on the square the piece left, and the
   tip lands inside the destination square.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: a drawing defect, proven with the browser's own geometry.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/Pages/Play.razor`, one line of `DESIGN.md`, and a screenshot in
`.tmp/testing`.
