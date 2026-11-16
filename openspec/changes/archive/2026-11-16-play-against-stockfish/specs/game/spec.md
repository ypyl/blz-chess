## ADDED Requirements

### Requirement: The page asks for a level before the first move

The app SHALL open on one page that lists the opponent levels from weakest to
strongest and SHALL start no game until one is chosen. The levels SHALL be named,
in this order: "No idea how to play", "Just learning", "Beginner", "Casual",
"Club player", "Expert".

#### Scenario: the page opens on the level list

- **WHEN** the app loads
- **THEN** the page shows the six level names and no game in progress

#### Scenario: choosing a level starts a game

- **WHEN** the player chooses "Casual"
- **THEN** the board appears with White to move and the page names the chosen level

### Requirement: The player moves pieces on the board

The app SHALL let the player select a piece and a destination square, SHALL
refuse a move the rules do not allow without passing the turn, and SHALL promote
a pawn that reaches its last rank to a queen.

#### Scenario: an illegal move is refused

- **WHEN** the player selects a white pawn and a square two ranks straight ahead
- **THEN** the piece stays where it was, the move is not recorded, and it is still the player's turn

#### Scenario: an illegal move leaves the king in check

- **WHEN** the player selects a piece whose move would expose the player's own king
- **THEN** the move is refused and the position does not change

#### Scenario: a pawn reaching the last rank becomes a queen

- **WHEN** a white pawn moves to the eighth rank
- **THEN** the square holds a white queen

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

### Requirement: The game ends and states the result

The app SHALL detect checkmate, stalemate, threefold repetition, the fifty-move
rule, and insufficient material, SHALL accept no further moves once one applies,
and SHALL name the result on the page. A "New game" action SHALL return the page
to the level list.

#### Scenario: checkmate ends the game

- **WHEN** the player delivers checkmate
- **THEN** the page states that the player won and the board accepts no further moves

#### Scenario: a new game asks for a level again

- **WHEN** the player chooses "New game" after a result
- **THEN** the page shows the level list and no board in play

### Requirement: Nothing leaves the browser

The game SHALL run entirely in the browser tab, SHALL send no position, move, or
result to any server, SHALL keep no game across a refresh, and the computer's side
SHALL only ever be Stockfish.

#### Scenario: a refresh loses the game

- **WHEN** the player reloads the page mid-game
- **THEN** the page shows the level list and no game in progress
