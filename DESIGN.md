# Design

The visual language of the board, and where the pieces come from. The spec states
behavior; this file states look.

## The page

One column, centred, on a dark page. Nothing competes with the board: no
navigation, no sidebar, no footer, no coordinates on the squares (a casual player
counts files better with clean squares than with labels in the corners).

- Page background `#1f1c18`, panels `#262220`, borders `#3a352f`.
- Text `#f4f1e8`, secondary text `#a29c90`, accent `#e8c25a`.
- Board light squares `#f0eed6`, dark squares `#7d945d`.
- The board is square, as wide as the column allows, capped at 34rem, with a soft
  shadow so it sits on the page rather than floating.
- Level list: one full-width button per level, weakest first, panel background,
  accent border on hover and focus.
- Selected square: inset accent ring. Legal destination: a small dark dot, or a
  ring when the destination holds a piece.

Type is the system sans stack. Headings are letterspaced tight; nothing else is
bold except the result line and the level name.

## Mobile

The same column at 96vw. The board takes the full width, the level list keeps its
full-width rows, and nothing needs a horizontal scroll at 390px.

## Pieces

The Cburnett set, twelve files in `wwwroot/pieces` named `wK.svg`, `wN.svg`, and so
on. Cburnett is CC BY-SA 3.0, compatible with this repository's GPLv3, and the
files come from the Lichess piece sets. Attribution lives in the README and here.

They are flat and drawn for clarity at small sizes, which is why they are used
instead of a 3D set: a board a player looks at for twenty minutes reads better
when the pieces are unambiguous at 40 pixels than when they are pretty at 200.

## Not here yet

Move animations, sound, coordinates, a theme switch, drag to move, and a
custom-built piece set. Each is a change of its own, and each has to earn its
place on a page that is currently one decision and one board.
