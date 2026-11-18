## MODIFIED Requirements

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
