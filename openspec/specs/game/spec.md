# game Specification

## Purpose
TBD - created by archiving change play-against-stockfish. Update Purpose after archive.

## Requirements

### Requirement: The player moves pieces on the board

The app SHALL let the player move a piece two ways: by selecting it and then choosing
a destination square, or by dragging it to a destination square and letting go. Both
SHALL work with a mouse and with a finger, and both SHALL be available whenever the
player is to move. The app SHALL refuse a move the rules do not allow without passing
the turn, and SHALL promote a pawn that reaches its last rank to a queen.

#### Scenario: an illegal move is refused

- **WHEN** the player selects a white pawn and a square two ranks straight ahead
- **THEN** the piece stays where it was, the move is not recorded, and it is still the player's turn

#### Scenario: an illegal move leaves the king in check

- **WHEN** the player selects a piece whose move would expose the player's own king
- **THEN** the move is refused and the position does not change

#### Scenario: a pawn reaching the last rank becomes a queen

- **WHEN** a white pawn moves to the eighth rank
- **THEN** the square holds a white queen

#### Scenario: a piece dragged to a legal square moves there

- **WHEN** the player drags a piece and lets go on a square that piece may move to
- **THEN** the piece is on that square, the move is recorded, and the computer answers

#### Scenario: a piece dropped where it may not go returns to its square

- **WHEN** the player drags a piece and lets go on a square that piece may not move to
- **THEN** the piece is on the square it started from, no move is recorded, and it is still the player's turn

#### Scenario: a drag on the computer's piece lifts nothing

- **WHEN** the player presses on a piece the computer owns and drags it
- **THEN** no piece is lifted, no destination is offered, and the position does not change

#### Scenario: a tap still moves a piece without dragging

- **WHEN** the player taps a piece and then taps a square it may move to
- **THEN** the move is played exactly as it was before dragging existed

### Requirement: The computer answers at the chosen level

After every player move the computer SHALL move with Stockfish, and its strength
SHALL follow the chosen level: "No idea how to play" mostly plays random legal
moves, and "Expert" plays the engine's best move for the position. A level SHALL
never play weaker than any level below it in the same position with the same
random seed.

#### Scenario: the weakest level often misses the best move

- **WHEN** the level is "No idea how to play" and the same position is played five times
- **THEN** at least two of the five replies differ from the engine's best move

#### Scenario: the strongest level plays the engine choice

- **WHEN** the level is "Expert"
- **THEN** the reply is the engine's best move for that position

### Requirement: Nothing leaves the browser

The game SHALL run entirely in the browser tab, SHALL send no position, move, or
result to any server, SHALL keep no game across a refresh, and the computer's side
SHALL only ever be Stockfish.

#### Scenario: a refresh loses the game

- **WHEN** the player reloads the page mid-game
- **THEN** the page shows the starting position at the weakest level, and the moves from before the reload are gone

### Requirement: The player can restart the game in progress

While a game is in progress the page SHALL offer a restart, and restarting SHALL
replace the game with a fresh one at the same level with White to move, and SHALL
record no result. Once a game has finished the page SHALL offer a new game instead,
which starts a fresh game at the level shown.

#### Scenario: restarting starts the same level again

- **WHEN** the player restarts in the middle of a game against "Casual"
- **THEN** the board shows the starting position with White to move and the level still reads "Casual"

#### Scenario: a restart records no result

- **WHEN** the player restarts in the middle of a game
- **THEN** the page states no winner and the game in progress is gone

#### Scenario: restart is offered only while a game is in progress

- **WHEN** a game has finished
- **THEN** the page offers no restart

### Requirement: The player can change the opponent level during a game

While a game is in progress the page SHALL let the player choose another level, and
choosing one SHALL change the opponent for its next move without changing the
position, the moves already played, or whose turn it is. The page SHALL name the
level now in play.

#### Scenario: changing the level keeps the position

- **WHEN** the player changes the level from "Casual" to "Expert" in the middle of a game
- **THEN** the pieces, the moves played, and the side to move are unchanged, and the page names "Expert"

#### Scenario: the new level answers

- **WHEN** the player changes the level and the computer moves next
- **THEN** that move is chosen at the level just chosen

### Requirement: The app starts a game as soon as it loads

The app SHALL start a game when it loads, at the weakest level, "No idea how to
play", with White to move, and SHALL NOT ask the player to choose a level before the
first move. The page SHALL name the level in play.

#### Scenario: the app starts a game on load

- **WHEN** the app loads
- **THEN** the board shows the starting position with White to move and the level reads "No idea how to play"

### Requirement: The game ends and names the result

The app SHALL detect checkmate, stalemate, threefold repetition, the fifty-move
rule, and insufficient material, SHALL accept no further moves once one applies,
and SHALL name the result on the page. A "New game" action SHALL start a new game at
the level shown, with no result carried over.

#### Scenario: checkmate ends the game

- **WHEN** the player delivers checkmate
- **THEN** the page states that the player won and the board accepts no further moves

#### Scenario: a new game starts again at the same level

- **WHEN** the player chooses "New game" after a result
- **THEN** the board shows the starting position with White to move, and the level still reads the same name

### Requirement: The player can take back the last move

While a game is in progress the page SHALL let the player take back their last move,
and taking a move back SHALL also un-play the computer's answer, so it is the
player's turn again. The position, the moves played, and any repetition state SHALL
be exactly what they were before the taken-back move was made.

#### Scenario: taking back the move restores the position

- **WHEN** the player has moved a pawn and the computer has answered, and the player takes the move back
- **THEN** the board shows the position before the pawn moved, it is the player's turn, and the taken-back moves are not in the game's history

#### Scenario: there is nothing to take back at the start

- **WHEN** no move has been played and the player tries to take a move back
- **THEN** the position does not change and no move disappears

#### Scenario: the take-back is not offered once the game is over

- **WHEN** a game has finished
- **THEN** the page offers no way to take a move back

### Requirement: The page shows the pieces each side has taken

The page SHALL show, for each side, the pieces that side has taken from the board, and
SHALL show each of them once. A piece SHALL leave that list when the move that took it is
taken back, and SHALL be shown as the piece it was when it was taken. The pieces one side
has taken SHALL be shown nearest that side, so that a glance at the board says who has
taken more.

The page SHALL also show the total value of the pieces it shows for each side, counting a
pawn as 1, a knight and a bishop as 3, a rook as 5, and a queen as 9. A side that has
taken nothing SHALL show no value rather than a zero.

The list and its value SHALL NOT change the board's place on the page or its size, before
or after a piece is taken, and the same SHALL hold while the app is still loading.

#### Scenario: a taken piece appears beside the board on the side that took it

- **WHEN** the player takes a black knight
- **THEN** the knight is on the player's side of the board and no longer on any square

#### Scenario: no taken piece is shown before the first capture

- **WHEN** a game starts
- **THEN** neither side shows a taken piece

#### Scenario: taking the move back returns the piece to the board

- **WHEN** the player takes back a move that took a piece
- **THEN** that piece is on the board again and no longer shown as taken

#### Scenario: a piece taken in passing leaves the board

- **WHEN** a pawn takes another pawn in passing
- **THEN** the taken pawn is shown as taken, although no piece moved onto its square

#### Scenario: a captured piece is shown as the piece it had become

- **WHEN** a pawn that had become a queen is later taken
- **THEN** the queen is shown as taken, not a pawn

#### Scenario: each side shows the value of what it has taken

- **WHEN** one side has taken a pawn, a knight, and a queen
- **THEN** that side shows 13 and the other side shows its own total

#### Scenario: a side that has taken nothing shows no value

- **WHEN** no piece has been taken
- **THEN** neither side shows a value

#### Scenario: the value follows a take-back

- **WHEN** the player takes back a move that took a queen
- **THEN** the taken value drops by 9

### Requirement: The player can see a suggested move

The page SHALL offer a switch that shows or hides a suggested move, and SHALL offer it
only while a game is in progress. The switch SHALL be off when the page loads, and its
state SHALL NOT be kept between visits.

While the switch is on and it is the player's turn, the page SHALL show the move the
engine considers best in the position on the board, chosen at the engine's full
strength and not at the strength of the opponent being played. The suggested move SHALL
be a move the rules allow the player to play.

A suggested move SHALL never be played, SHALL never change the position, the moves
played, the level, or the result, and SHALL never take a click away from the board. The
page SHALL show no suggestion while the computer is thinking or when the game is over,
and SHALL show a suggestion for the new position once the computer has answered.

#### Scenario: no suggested move is shown until the switch is on

- **WHEN** the page loads
- **THEN** the switch is off and the board shows no suggested move

#### Scenario: the suggested move is asked for at full strength

- **WHEN** the player turns hints on while playing the weakest level
- **THEN** the engine is asked for its best move at full strength, not at the weakest level's strength

#### Scenario: the suggested move is one the player may play

- **WHEN** a suggestion is shown
- **THEN** it is one of the moves the rules allow in the position on the board

#### Scenario: switching hints off hides the suggested move

- **WHEN** the player switches hints off
- **THEN** no suggested move is shown

#### Scenario: a suggested move changes nothing in the game

- **WHEN** the player turns hints on and ignores the suggestion
- **THEN** the position, the moves played, the level, and the result are exactly what they were

#### Scenario: the suggestion follows the position after the computer answers

- **WHEN** the player moves and the computer answers
- **THEN** a suggestion for the new position is shown and the suggestion for the old one is gone

#### Scenario: no suggestion is offered once the game is over

- **WHEN** the game has ended
- **THEN** the page offers no switch for hints and shows no suggested move
