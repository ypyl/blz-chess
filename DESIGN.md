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
- Squares meet with no separator: the two colours are the only division, as on a
  real board. Every cell is the same square whatever stands in it, which is a
  property of the grid, not of the pieces.
- Level list: one full-width button per level, weakest first, panel background,
  accent border on hover and focus.
- The header's action follows the state: "Restart" while a game runs (same level,
  fresh board, no result) and "New game" once it is over (back to the level list).
  One slot, so the header has one shape.
- The level is a list while a game runs, so the opponent can be changed without
  losing the position. Once the game is over it is a plain label: the choice no
  longer does anything, and pretending otherwise would be a trap.
- In a game the header holds the level, the status, and that action in that order:
  level name, then the message, then the button. The status slot exists in every
  state, so "Your move", "Thinking" and the result take the same room and the board
  never moves. On a phone the message gets its own row, still in every state.
- Selected square: inset accent ring. Legal destination: a small dark dot, or a
  ring when the destination holds a piece.

Type is the system sans stack. Headings are letterspaced tight; nothing else is
bold except the result line and the level name.

## Mobile

The same column at 96vw. The board takes the full width, the level list keeps its
full-width rows, and nothing needs a horizontal scroll at 390px.

## The loading screen

Before the app boots, the page shows its own mark over one rank of the board, with
the percentage underneath. The rank fills left to right as the runtime arrives, in
the accent colour, and the mark bobs gently; `prefers-reduced-motion` stops the bob.
Blazor publishes its progress as two custom properties on the document, so the rank
and the number need no script of ours.

The elements are the page's own markup in `index.html`, not a component: Blazor
replaces that subtree when it takes over, which is exactly when the wait ends.

## The mark

The icon is the knight from the same piece set, cream on the board's green, filling
the tile so it still reads at 16 pixels in a tab. It is drawn once as
`wwwroot/icon.svg` and rendered from that file to `icon-32.png` (tab),
`icon-180.png` (apple-touch), `icon-192.png` and `icon-512.png` (manifest). The SVG
is the source; the PNGs are output and can be regenerated from it at any time.

No new artwork and no second identity: the app is its pieces.

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
