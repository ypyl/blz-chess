# Make the taken pieces readable on a phone

L0

## Why

The trays sit on the page background, which is nearly black, and a black piece on a
nearly black line is a smudge. On a phone the line is 28 pixels tall and the pieces are
24, so the only thing distinguishing a taken black pawn is its outline against the page.

## What Changes

The tray gets its own background: the light square colour, a rounded shelf the width of
the board. Black pieces read at 17:1 against it, and white pieces keep their dark
outlines, with a soft shadow to lift them off the cream. The shadows are gone from the
pieces themselves, which were drawn at 70% opacity: dimming a piece is what made the
taken pieces look faded in the first place.

On a phone the shelf is taller, 36 pixels instead of 28, so the pieces are drawn at 32
pixels rather than 24. The board's height budget recalculates from the same variable, so
the board shrinks by exactly the extra shelf and still does not move between views.

## Routing

0. **Observable behavior changes?** No. Nothing about what the app does changes; the
   same pieces appear in the same places, on a different colour.
1. **Undoable in one commit?** Yes: three declarations in one stylesheet.
2. **A machine can prove it?** Yes. Contrast is arithmetic, not opinion: `verify.sh`
   gains a check that the tray's background reaches 4.5:1 against a black piece, which
   is the ratio the old near-black background failed.
3. **Does anything outside depend on it?** No spec, no ADR, no contract.

Level **L0**: presentation, reversible, provable by a calculation, and nothing depends
on it.

## Capability

`game` is untouched. The requirement "The page shows the pieces each side has taken"
says the pieces are shown, not on what colour.
