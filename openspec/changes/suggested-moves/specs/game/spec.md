## ADDED Requirements

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
