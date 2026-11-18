# Design: replay, not reverse

## How a take-back works

`Position.ApplyUnchecked` cannot be run backwards: it clears castling rights, moves
rooks for castling, removes en-passant pawns from squares nobody moved to, and resets
the halfmove clock. Reversing all of that is a second move generator with its own
bugs waiting.

The game keeps its initial position, so a take-back is a replay: drop the moves,
apply the rest from the start, and rebuild the repetition keys from the sequence.
Every move already passed the legality filter when it was played, and `ApplyUnchecked`
is deterministic, so the rebuilt position is the position, not an approximation.
Games here are tens of moves, so the cost is nothing.

The public shape: `Game.TrimMoves(int)` drops the last N moves and rebuilds;
`GameSession.UndoPlayerTurn()` refuses unless it is the player's turn, so the
computer's reply and the player's move go together; `GameFlow.UndoLastMove()` refuses
unless a game is running.

## Why the pair and not one move

Taking back only the player's move leaves the computer's reply in place, and the
computer would simply answer again, or answer a different move with the level
changed, and the "take-back" would be a shuffle. The player gets their move back with
their turn, which is what "let me try that again" means.

## The header

The action area becomes a pair while a game runs: "Undo" and "Restart", one shape in
every playing state, with Undo inert at the opening. When the game is over the pair
becomes "New game" alone: a result stays a result, and undoing into a finished game
is a different feature with its own questions.

The buttons sit in the same slot Restart occupied, so the status keeps the middle and
the board does not move.

## Testing

- Restore: play e4, let the scripted engine answer e5, take back; assert the FEN, the
  move list, the side to move, and the stage.
- Opening: no moves, the take-back is refused, position unchanged.
- Not the player's turn: after the player's move and before the engine's reply, the
  take-back is refused.
- Repetition is real after a rebuild: shuffle two knights in and out twice, take back
  the last turn, play the same shuffle again, and the game still draws by threefold,
  which fails if the rebuilt key list is a shortcut rather than the truth.
- Finished game: the flow refuses the take-back.
