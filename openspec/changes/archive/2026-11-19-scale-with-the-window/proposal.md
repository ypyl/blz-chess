# Scale the whole page, not only the board

L0

## Why

When the window gets shorter, the board shrinks and nothing else does. At 1280x640 the
board goes from 544 to 387 pixels while the buttons stay 33 tall and the shelves 28, so
the frame around the board looks oversized and the board looks cramped inside it. On a
landscape phone the board is 137 pixels inside a header built for a 544-pixel board.

## What Changes

One number scales the app: the root font-size. Every size in the stylesheet is already in
rem, so the board, the header, the buttons, the type, the shelves, and the reserved lines
between them all grow and shrink by the same factor, which is what "the app is this big"
means.

The scale is the design size until the window cannot hold it, then the fraction of a
full-size app that still fits. It stops shrinking at 0.8 of the design size, because type
below that is unreadable; under that point the board is capped by its own height rule, as
it is today, and the chrome around it is proportionally larger.

Nothing grows past the design size. A larger screen gets the same board it gets today,
centred with space around it, which is a separate decision and not this fix.

## Routing

0. **Observable behavior changes?** No requirement moves. What changes is the size of
   things at a given window, which is layout, not behavior.
1. **Can it be undone easily?** Yes: one rule, and the same rule in the `dvh` block.
2. **Can a machine prove it correct?** Yes: measure the root font-size, the board, a
   button, a shelf, and the dropdown at several window sizes, and check they move by the
   same factor. Also check the board is identical before and after the app takes over, at a
   scaled size, and that no size scrolls.
3. **Does anything outside depend on it?** No spec, no ADR.

Level **L0**: presentation, one rule, reversible, and measurable.

## Capability

`game` is untouched. No requirement mentions sizes.
