# Show the computer's move as an arrow

L0

## Why

After the computer moves, a player who looked away for a second has to hunt for what
changed. The move is legal and instant, but invisible: nothing on the board says
which piece came from where.

## What Changes

The computer's last move is drawn over the board as an arrow in the accent colour:
a shaft between the centres of the from and to squares, with a head on the
destination. It appears when the computer moves, stays through the player's whole
turn, and is cleared by the player's own move, a take-back, a restart, or a new game.
It takes no clicks and hides nothing: the geometry is computed in the page from the
move's squares, over an overlay that never touches the squares underneath.

## Routing

0. **Observable behavior changes?** The same move is now marked visually. No spec
   requirement moves: this is how "the computer answers" is shown, not what it does.
1. **Undoable in one commit?** Yes: one field, one overlay element, one geometry
   method, one CSS block.
2. **Can a machine prove it correct?** Yes: the arrow's line starts on the exact
   centre of the from-square (measured: `450,150` for e7) and points at the
   destination (e5), it disappears after a take-back, and the squares beneath stay
   clickable.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: presentation of a behavior the spec already owns.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/Pages/Play.razor`, `src/BlzChess.Web/wwwroot/css/app.css`, and the
look section of `DESIGN.md`.
