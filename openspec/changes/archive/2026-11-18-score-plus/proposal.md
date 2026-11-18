# Put a plus sign before the material count

L0

## Why

A shelf's number is a total, and everything else on that shelf's row is a piece that is
gone. "+13" reads as what a side has gained, "13" reads as a count of something on the
page. The sign costs one character and removes the doubt.

## What Changes

The value on each shelf is drawn with a leading plus: "+1", "+13".

## Routing

0. **Observable behavior changes?** No. The requirement says the page shows the total
   value of what each side has taken; it does not say how the number is written, and the
   number is the same number.
1. **Undoable in one commit?** Yes: one interpolated string per shelf.
2. **A machine can prove it?** Yes: the rendered text is read from the page and compared
   with the sign, in a browser.
3. **Does anything outside depend on it?** No.

Level **L0**: presentation, no behavior, one commit to reverse.

## Capability

`game` is untouched. The requirement "The page shows the pieces each side has taken"
still holds, unchanged.

## Assumption

The sign goes on both shelves, not only on the leading one. A "+" that appears and
disappears with the lead would be a second behavior (a comparison), not a format, and the
screen-reader label already says what each total is worth in words.
