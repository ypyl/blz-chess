## MODIFIED Requirements

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
