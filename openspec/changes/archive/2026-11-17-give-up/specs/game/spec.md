## ADDED Requirements

### Requirement: The player can restart the game in progress

While a game is in progress the page SHALL offer a restart, and restarting SHALL
replace the game with a fresh one at the same level with White to move, and SHALL
record no result. Once a game has finished the page SHALL offer a new game instead,
which returns to the level list.

#### Scenario: restarting starts the same level again

- **WHEN** the player restarts in the middle of a game against "Casual"
- **THEN** the board shows the starting position with White to move and the level still reads "Casual"

#### Scenario: a restart records no result

- **WHEN** the player restarts in the middle of a game
- **THEN** the page states no winner and the game in progress is gone

#### Scenario: restart is offered only while a game is in progress

- **WHEN** the page shows the level list, or a game has finished
- **THEN** the page offers no restart
