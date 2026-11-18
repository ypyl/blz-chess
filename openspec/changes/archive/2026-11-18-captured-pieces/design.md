# Show the pieces that have been taken

## Where the list comes from

`Game` gains `Captured`, a list of the pieces taken, in the order they were taken. It is
filled in the one place a move is applied, so the live path and the replay that a
take-back already runs record it the same way and cannot disagree.

One application site matters for correctness: a move onto an occupied square takes what
is standing there, and a pawn moving diagonally onto an empty square is taking in
passing, so the piece removed is the pawn beside it rather than anything on the target
square. `Position.ApplyUnchecked` handles both, so the record is read from the position
*before* the move is applied.

The record is not derived by counting pieces and comparing with the opening, because
that cannot tell a captured queen from a pawn that promoted into one. Counting would
report a pawn and hide a queen, which is exactly the piece a player wants to see.

## Where it is drawn

Two trays inside the shell column: the computer's above the board, the player's below.
The computer's pieces are at the top of the board, so the pieces it took sit nearest its
own side, and the same for the player at the bottom. A tray shows pieces in value order,
pawn to queen, so the same row means the same thing game after game.

Each tray is one line tall and that line is reserved whether or not it holds a piece.
Nothing about the tray's content can change the shell's height, which is what keeps the
board still: the shell is centred vertically, so a row that appeared would push the
board up. The board's height cap loses the trays' height and the two extra gaps between
the shell's children, in one variable rather than in a sum written out twice.

The loading screen reserves the same two lines around its own board. It is the game view
with the pieces still arriving, and a takeover that moved the board by a plate would be
the same defect the earlier changes fixed for the status line.

## Accessibility

The trays carry a label naming whose pieces they are and which pieces, in words, so a
screen reader gets the information the pictures carry. The pictures themselves are
decorative: the label already says it.

## What is not built

No material score, no "the computer is ahead by a knight" sentence, no animation of a
piece sliding to the tray, no sorting control. The board is the game; this is one line
of context.
