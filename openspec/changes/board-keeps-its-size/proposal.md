# The board keeps its size; the page scrolls instead

L0

## Why

The board's width was capped by the window's height, so a short window handed it a smaller
square: 435 pixels at 1280x640, 188 at 844x390. The board is the game. Shrinking it to make
a header and two shelves fit is the wrong thing to give up, and the human asked for the
height to stop deciding it.

## What Changes

The board is the width it has: `min(96vw, 34rem)`. The height of the window no longer
enters the expression, and neither does the frame around it. The app's scale follows the
same rule — width only — so the type, buttons, trays and board stay one size.

When the window is too short for the board and its frame, the page scrolls. Measured:

| window | board | page height | viewport | scrolls |
|---|---|---|---|---|
| 1280x800 | 544 | 800 | 800 | no |
| 1280x640 | 544 | 787 | 640 | yes, 147 px |
| 1024x560 | 544 | 787 | 560 | yes |
| 390x844 | 374 | 844 | 844 | no |
| 844x390 | 544 | 787 | 390 | yes, 397 px |

The reserved lines for the trays and the status stay, so a capture or a change of status
still cannot move the board; they simply no longer take height away from it.

## Routing

0. **Observable behavior changes?** Layout only, and it is visible: a short window now
   scrolls where it used to shrink the board. No requirement in the spec mentions size or
   scrolling.
1. **Can it be undone easily?** Yes: the height term in one expression, in two rules.
2. **Can a machine prove it correct?** Yes: the board's width at five window sizes, and
   whether the page scrolls at each.
3. **Does anything outside depend on it?** No.

Level **L0**: one expression, reversible, measured.

## Cost worth naming

On a phone held sideways the board is 544 wide in a 390-tall window, so it cannot be seen
whole: the page scrolls 397 px. That is the arithmetic of a fixed board in a short window,
not a bug, and it is the trade the human chose. If sideways phones matter more than a
fixed board, the height term comes back for windows shorter than the board needs.
