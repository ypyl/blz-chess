## ADDED Requirements

### Requirement: The app starts a game as soon as it loads

The app SHALL start a game when it loads, at the weakest level, "No idea how to
play", with White to move, and SHALL NOT ask the player to choose a level before the
first move. The page SHALL name the level in play.

#### Scenario: the app starts a game on load

- **WHEN** the app loads
- **THEN** the board shows the starting position with White to move and the level reads "No idea how to play"

## MODIFIED Requirements

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

### Requirement: Nothing leaves the browser

The game SHALL run entirely in the browser tab, SHALL send no position, move, or
result to any server, SHALL keep no game across a refresh, and the computer's side
SHALL only ever be Stockfish.

#### Scenario: a refresh loses the game

- **WHEN** the player reloads the page mid-game
- **THEN** the page shows the starting position at the weakest level, and the moves from before the reload are gone

## REMOVED Requirements

### Requirement: The page asks for a level before the first move

**Reason**: The level is a control in the header that can be changed at any time, so a
screen asking for it before the first move is a step nobody needs. The game now starts
on load at the weakest level, which the added requirement states.

### Requirement: The game ends and states the result

**Reason**: Replaced by "The game ends and names the result", which keeps every promise
this requirement made and changes only what "New game" does now that there is no level
list to return to.

## ADDED Requirements

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
