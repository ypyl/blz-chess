# Put the material score on the trays

## The scale, in one place

`BlzChess.Core.Material` holds the numbers and the sum. The page never adds anything up:

- `ValueOf(PieceType)` — pawn 1, knight 3, bishop 3, rook 5, queen 9, king 0. A king is
  0 rather than an exception because the tray can never hold one, and a function that
  cannot be called should not need a guard.
- `Taken(Game, Color)` — the value of the pieces of that colour that are off the board.

Both are pure, so the arithmetic is a unit test away from the page, and the page is left
with a call and a span.

## On the shelf

The tray is a flex row of piece images. The number is one more child with `margin-left:
auto`, which pushes it to the far end of the same row: no second container, no absolute
positioning, and nothing that can disagree with the shelf's height. The line is already
reserved, so the number cannot move the board.

A tray with nothing in it renders no number. An empty opening board with two zeros on it
would be noise that says nothing a player wants to read.

The number is drawn in the page's ink colour, which is what a reader needs against the
light shelf: the shelf is already the light square colour, and the ink is the dark end of
the palette, so the pair is legible without a second thought. `verify.sh` holds the shelf
against a black piece, which is the harder of the two.

## For a screen reader

The tray's label already lists the pieces; it gains the total, so the same information
arrives in the same breath: "You have taken: black pawn, black queen, worth 10." The
number itself is decorative once that sentence exists, so the span is hidden from the
accessibility tree and the label carries it.
