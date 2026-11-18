# Drag a piece to its square

## The interaction

Pointer events on the board, not HTML5 drag and drop. `dragstart`/`drop` never fire
for a finger on a touch screen, and the phone is the first target. `pointerdown`,
`pointermove`, `pointerup`, and `pointercancel` cover mouse, finger, and pen with one
set of handlers.

The drag begins on `pointerdown` over a square holding one of the player's pieces,
during the player's turn, while the engine is idle. The press does not change the
selection: selection stays on the click path so a tap keeps working exactly as it
does today.

A press becomes a drag only after the pointer has travelled 6 CSS pixels from where
it went down. Below that threshold nothing is lifted and `pointerup` is ignored, so
the click that follows runs the old code and a tap can never be mistaken for a
misfired drag. Above it, the piece is lifted, the selection and the legal
destinations are set as a click would set them, and every square that can receive
the piece shows its marker.

`pointerup` reads the square under the pointer. If it is one of the lifted piece's
destinations, the move is played through the same path a click uses; the difference
is the input, not the rules. Anywhere else, the piece returns and the selection
stays, so the player can immediately tap a destination instead. `pointercancel`
drops the drag without a move.

## Where the pointer is

Square arithmetic moves into `BlzChess.Core` as `BoardGeometry.SquareAt(x, y, side)`:
`x` and `y` measured from the board's top-left corner, `side` the board's edge, all in
CSS pixels. File comes from the first, rank from the second with rank 8 at the top,
and a point outside the board returns `Squares.None`. Pure, no browser, so the cases
that matter (corners, edges, both board sizes, a point past the edge) are unit tests.

The component gets the board's rectangle from the DOM through one small JS helper,
`wwwroot/js/pointer.js`, which also sets and releases pointer capture. Capture matters
because a drag that wanders off the board would otherwise stop delivering
`pointermove` and never deliver its `pointerup`, leaving the piece lifted forever.

The lifted piece is an `<img>` inside the board, absolutely positioned under the
pointer, centred on it, and `pointer-events: none` so it never swallows the event that
is tracking it. It is drawn half a square above and left of the pointer so the piece
appears held by its middle rather than by its corner.

## Stopping the page from panning

The board gets `touch-action: none`. Without it the browser claims a finger drag for
scrolling and sends `pointercancel` instead of the moves, so the drag dies on the one
device where it matters. The cost is that pinch-zoom over the board no longer works.
The shell has no scroll and the only text is one line of status, so nothing on that
screen needs a zoom, but it is a real loss and it is recorded as a marker in the
stylesheet:

`shortcut: the board refuses touch-action so a drag can own a finger; revisit if pinch-zoom over the board is ever wanted, which needs per-gesture touch-action support the browsers do not have.`

## What is not built

No drop-target highlight beyond the markers already drawn for a selection, no
animation of the piece returning, no keyboard drag. Restyling is not behavior and
does not belong in this change.
