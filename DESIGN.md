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
- The header's actions follow the state: "Undo" and "Restart" while a game runs
  (undo is inert at the opening, when there is nothing to take back) and "New game"
  alone once it is over. The pair sits in one slot, so the header has one shape.
- The level list is always enabled: mid-game it changes the opponent's next move,
  after a result it changes the next game's.
- The header holds the level and the actions, nothing else. The status lives under
  the board, centered, in a slot with its own reserved height, so "Your move",
  "Thinking" and the result take the same room and the board never moves: the shell
  is centred vertically, and a line below the board that grew would shift the board
  above it.
- Selected square: inset accent ring. Legal destination: a small dark dot, or a
  ring when the destination holds a piece.
- The computer's last move is drawn over the board as an arrow in the accent colour,
  from the centre of the square the piece left to the centre it landed on, with a
  head on the destination. The overlay is a child of the board, so its coordinate
  system is the board's: `inset: 0` against a sibling would have spanned the window
  and pinned the arrow to the middle of the page instead. It stays through the player's whole turn and is cleared by
  the player's own move, a take-back, a restart, or a new game. It never takes a
  click.
- Two trays of taken pieces flank the board: the computer's above it, on the side of
  the pieces it still has, and the player's below. Each is one reserved line tall
  whether or not it holds anything, so a capture cannot change the shell's height and
  push the board: the shell is centred, and a row that appeared would move the board
  up. Pieces are listed smallest first and drawn smaller than a piece on the board, so
  the board stays the thing you look at. The shelf is the light square colour, not the
  page: a taken black piece on a near-black page is a black shape on black, which is
  how it shipped once. A black piece reads at 17.9:1 on the shelf, and a white piece
  keeps the dark outline its artwork carries. `verify.sh` holds that ratio. Each shelf
  carries the total value of the pieces on it, at its right end, pushed by the row rather
  than positioned: the number a player wants is "who is ahead", and the pieces to work it
  out are already on the shelf. A pawn counts 1, knight and bishop 3, rook 5, queen 9.
  An empty shelf shows no number, so an opening board stays quiet.
- A "Hints" switch in the header turns on a suggested move: the engine's best move for
  the player, drawn as a dashed, thinner, dimmer version of the same arrow, so "it
  played there" and "you could play there" cannot be confused, including for a reader
  who cannot separate the two colours. Asked at the coach's strength, never the
  opponent's. The switch is off on load and not remembered; a hidden line names the
  move for a screen reader, because an arrow says nothing aloud.
- A piece can be picked up and dropped on its destination. Pressing one of the
  player's own pieces takes hold of it once the pointer has travelled 6 CSS pixels,
  and its legal destinations light up while it travels; letting go on one plays the
  move, and letting go anywhere else puts the piece back. The piece in the air is
  drawn under the pointer, centred on it, at the board's own cell size, and the square
  it came from stays visible with its piece dimmed. A press that never travels stays a
  tap, so tapping a piece and tapping its destination works exactly as before.

Type is the system sans stack. Headings are letterspaced tight; nothing else is
bold except the result line and the level name.

## Mobile

The same column at 96vw. The board takes the full width, the header wraps the status
onto its own row, and nothing needs a horizontal scroll at 390px. The board is also
capped by the viewport height in dynamic units, so a landscape phone holds the whole
board without scrolling either.

The app installs as a PWA: the manifest carries the name, the standalone display,
the app's colours, and the three icon sizes an install prompt asks for, and
`scripts/verify.sh` refuses to let any of that drift.

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
