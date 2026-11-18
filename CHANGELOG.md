# Changelog

One entry per user-facing change, newest first, under `## Unreleased`. The rules
are in `AGENTS.md` under **Versions and changelog**.

## Unreleased

### Added

- A "Hints" switch shows the move the engine considers best for your position, as a
  dashed arrow on the board. It is asked at full strength whatever opponent you play,
  it never plays the move for you, and it is off unless you turn it on.
- The pieces each side has taken are shown beside the board, the computer's above it
  and yours below, so the material on the board and off it can be read at a glance.
  They sit on a pale shelf, which makes the black ones visible on a phone. Each shelf
  shows the value of the pieces on it, so who is ahead in material needs no arithmetic.
- Pieces can be dragged to their square with a mouse or a finger, as well as tapped.
  A piece's legal destinations appear while it is in the air, and letting go anywhere
  else puts the piece back.

### Changed

- The whole page scales with the window. In a short window the header, the buttons, the
  type, and the shelves shrink with the board, instead of the board shrinking alone inside
  a frame sized for a bigger screen.
- The levels are called Random, Learner, Beginner, Casual, Club, and Expert. The old
  "No idea how to play" made the dropdown as wide as a sentence and took the room the
  buttons needed. Nothing about how they play changed.
- The app starts a game as soon as it opens, at the weakest level, instead of asking
  for a level first. "New game" after a result starts a fresh game at the level shown,
  and the level list in the header can change the opponent at any time.

### Added

- Take back your last move. The computer's answer goes with it, the position and the
  history return to what they were, and it is your turn again.
- Change the opponent level in the middle of a game. The position, the moves played,
  and whose turn it is stay as they are; only the next computer move is chosen at the
  new level.

- Restart a game in progress: the board goes back to the start at the same level,
  and no result is recorded.

### Changed

- The app has its own icon, a knight on the board's green, instead of the Blazor
  logo the template shipped.

### Fixed

- The computer's move is shown as an arrow on the board, from the square the piece
  left to the one it landed on, so a glance is enough to see what the opponent did.
  The arrow is drawn inside the board: it first shipped anchored to the window and
  floated in the middle of the page.
- The status line, "Your move" and the result, moved from the header to under the
  board, where a player looks while playing. The header keeps the level and the
  actions.
- The phone layout was silently broken: a stray brace in the stylesheet stopped
  every rule after it, including the one that gives the status its own row. The board
  is now also capped by the viewport height, so a landscape phone does not scroll.

### Fixed (older)

- The board no longer flashes the words "white pawn" on its squares while the piece
  images load: the images are decorative, the square already has a label, and the
  twelve of them are preloaded with the page.

- Knights were drawn as kings, because the piece image was chosen by the first
  letter of the piece name and Knight and King share it.

### Added

- Play a game against Stockfish. The page opens on six named levels, from "No idea
  how to play" to "Expert", and choosing one starts a game where you are White.
  The rules refuse illegal moves, the game announces checkmate, stalemate,
  repetition, the fifty-move rule, and insufficient material, and "New game"
  returns to the level list.

- The app: a Blazor WebAssembly PWA shell that installs and serves a BlzChess
  page, with a C# chess rules library and a test project behind it.
- A pipeline that builds, tests, and publishes the site to GitHub Pages on every
  push to `main`.
