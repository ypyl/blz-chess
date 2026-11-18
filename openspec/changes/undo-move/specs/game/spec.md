## ADDED Requirements

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
