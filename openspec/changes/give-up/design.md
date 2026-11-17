# Design: one slot, two actions

## Why the action changes instead of multiplying

The previous change removed everything that appeared below the board, because the
page moved at the moment a game ended. A second button in the header would put the
same problem sideways: it would come and go, and the level name and status would
slide around it.

One slot with two labels keeps the header one shape in every state:

| State | Button | What it does |
|---|---|---|
| Level list | none | there is no game |
| Playing | "Restart" | fresh game, same level, White to move, no result |
| Finished | "New game" | back to the level list |

The sequence reads in order: restart while you are playing, and change opponent once
you are done.

## Why the restart is a flow method

`GameFlow` already owns the level and the session, so a restart is
`ChooseLevel(Level)` with the current level and a fresh seed. Doing it in the
component would put a state rule in the markup, where the tests cannot reach it.

The restart also has to be refused off-stage: on the level list there is no game to
restart, and after a result the button is "New game".

## What the player sees

The board goes back to the starting position, the level name stays, the status
returns to "Your move", and no winner was ever named. The board's position on the
page does not move, because nothing about the header's shape changes.

## Testing

`GameFlowTests`: a restart keeps the level, empties the move list, returns the start
position, leaves no result, and does nothing from the level list or after a finished
game. The browser check reads the button label in playing and finished states.
