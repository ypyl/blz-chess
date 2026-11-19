# Scale by the narrow side too

L0

## Why

The last change scaled the app from the window's height alone. A narrow window has height
to spare, so the scale stayed at its design size while the board was capped by width and
shrank alone: at 420x900 the board was 403 pixels, 74% of its frame, and at 390x844 it was
374, 69%. That is the same complaint the previous change was meant to fix, still true on a
phone.

## What Changes

The scale is the smaller of the board's two caps — width or height — divided by the board's
design width. Both caps are the board's own, so they cannot disagree with it: a narrow
window binds on width, a short one on height, and whichever binds is the size of
everything.

| window | board | frame | scale |
|---|---|---|---|
| 1280x800 | 544 | 544 | 1.00 |
| 1280x640 | 435 | 435 | 1.00 |
| 420x900 | 403 | 435 | 0.93 |
| 390x844 | 374 | 435 | 0.86 |

## Routing

0. **Observable behavior changes?** No requirement moves. Sizes at a given window change;
   the game, the board's contents, and every rule stay as they are.
1. **Can it be undone easily?** Yes: one expression, in two places.
2. **Can a machine prove it correct?** Yes: the board's width divided by the frame's width
   is a number, measured at six window sizes.
3. **Does anything outside depend on it?** No.

Level **L0**: presentation, one expression, proven by measurement.

## Trade-off worth naming

On a 390-pixel-wide phone the type is now 12.8 pixels and the buttons 27 tall, where they
were 16 and 33. Proportion costs legibility, and the floor at 0.8 is where it stops: below
that the board takes whatever the fixed frame leaves. A phone that narrow is where the two
aims genuinely conflict.
