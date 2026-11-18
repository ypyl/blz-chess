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
- There is no screen between loading and playing: the page opens on a board at the
  weakest level, and the level is a list in the header from the first move.
- The header's action follows the state: "Restart" while a game runs (same level,
  fresh board, no result) and "New game" once it is over (fresh board at the level
  shown). One slot, so the header has one shape.
- The level list is always enabled: mid-game it changes the opponent's next move,
  after a result it changes the next game's.
- The header holds the level, the status, and that action in that order: level name,
  then the message, then the button. The status slot exists in every state, so "Your
  move", "Thinking" and the result take the same room and the board never moves. On a
  phone the message gets its own row, still in every state.
- Selected square: inset accent ring. Legal destination: a small dark dot, or a
  ring when the destination holds a piece.

Type is the system sans stack. Headings are letterspaced tight; nothing else is
bold except the result line and the level name.

## Mobile

The same column at 96vw. The board takes the full width, the header wraps the status
onto its own row, and nothing needs a horizontal scroll at 390px.

## The loading screen

Before the app boots, the page shows the board being set up: an empty board of the
app's cream and green, and the thirty-two pieces arriving one at a time from the
white back rank outward, like someone placing them. At the end the board is set,
which is the position the game starts from, so the loading screen and the game share
one picture. The pieces are the indicator; there is no text, no number, nothing else
on the page.

The loading view is laid out as the game view: the same shell, a header row whose
three slots are invisible placeholders, and the same board. Its squares follow the
same parity as the game board, a8 light and a1 dark, and `scripts/verify.sh` asserts
that on the static markup because nothing derives it for them. So the board is the
same size and in the same place before and after the app takes over, and nothing
jumps or resizes at the moment loading ends.

Blazor publishes its progress as a custom property on the document. A short inline
loop reads it, places that share of the thirty-two pieces, and stops the moment
Blazor replaces the loading markup. The pieces are the ones already preloaded for
the board, so nothing extra is fetched.

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
