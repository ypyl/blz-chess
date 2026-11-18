## MODIFIED Requirements

### Requirement: The computer answers at the chosen level

After every player move the computer SHALL move with Stockfish, and its strength
SHALL follow the chosen level: "Random" mostly plays random legal
moves, and "Expert" plays the engine's best move for the position. A level SHALL
never play weaker than any level below it in the same position with the same
random seed.

#### Scenario: the weakest level often misses the best move

- **WHEN** the level is "Random" and the same position is played five times
- **THEN** at least two of the five replies differ from the engine's best move

#### Scenario: the strongest level plays the engine choice

- **WHEN** the level is "Expert"
- **THEN** the reply is the engine's best move for that position

### Requirement: The app starts a game as soon as it loads

The app SHALL start a game when it loads, at the weakest level, "Random", with White to
move, and SHALL NOT ask the player to choose a level before the
first move. The page SHALL name the level in play.

#### Scenario: the app starts a game on load

- **WHEN** the app loads
- **THEN** the board shows the starting position with White to move and the level reads "Random"
