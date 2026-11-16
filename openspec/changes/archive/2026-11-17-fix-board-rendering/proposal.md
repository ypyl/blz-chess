# Draw the right piece in every square, and make cells look like cells

L0

## Why

Two things are wrong on the board a player sees.

Every knight is drawn as a king. The piece image was chosen by the first letter of
the piece name, and Knight and King share it, so both resolved to `wK.svg`. The
lucky half is that the board still has two of each; the unlucky half is that a
player sees four kings a side and no horses.

Empty squares read as patches of colour rather than cells. The two colours are
correct (measured: `a1` is `#7d945d`), but with no separation between squares the
empty half of the board has no cell edges at all, which is what makes it look
unfinished next to the piece rows.

## What Changes

`PieceFiles` in `BlzChess.Core` maps a piece to its file name and its alt text, one
place with an explicit letter per type, and the page calls it. A test asserts all
twelve pieces map to twelve distinct files, which is the check that would have
caught the collision: two types sharing an initial cannot pass it.

Squares are separated by a one-pixel line of the board's own background, so an
empty square reads as a square. `DESIGN.md` says so, since that is where the board's
look is owned.

## Routing

0. **Observable behavior changes?** The board shows the correct pieces. No spec
   requirement moves: `openspec/specs/game/spec.md` says the player moves pieces,
   not which drawing a knight uses.
1. **Undoable in one commit?** Yes: one mapping helper, one CSS line.
2. **A machine can prove it correct?** Yes. `TwelvePiecesTwelveFiles` fails the
   moment two pieces share a file, and the browser check reads the `src` of each
   square.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0** by `fix-bug`: the knight is a defect that restores intended behavior;
the cell line is the smallest visual change that makes the board read correctly.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Core/PieceFiles.cs` is new, `Play.razor` uses it, `app.css` gains the
square separation, and `DESIGN.md`, `CHANGELOG.md` follow.
