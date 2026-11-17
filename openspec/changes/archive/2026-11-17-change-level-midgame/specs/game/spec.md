## ADDED Requirements

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
