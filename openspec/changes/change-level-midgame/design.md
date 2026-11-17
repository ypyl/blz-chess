# Design: the level is a control, not a label

## Where the change lives

`GameSession` holds the level it was created with, which is why it could not change.
It gains a setter guarded by a method, and `GameFlow` exposes `ChangeLevel` with the
same stage guard `Restart` uses: only while a game is in progress. Nothing else about
the session moves, because the position and the move list are untouched.

## What the header looks like

```
[ Casual v ]          Your move.          [ Restart ]
```

A native `<select>` in the first slot, in every state where the header exists, so
the header keeps one shape. While the game runs it changes the opponent; once the
game is over it is disabled, which keeps the finished game readable next to its
result without pretending the choice still does something.

The select is keyed on the current level, so a level chosen from the list is what
the control shows afterwards even though Blazor would otherwise leave the old value
in place.

## What changes for the engine

Only the next request. A move already being computed at the old level finishes at
the old level, which is honest and keeps the code free of cancellation for a case a
player cannot feel.

## Testing

- `ChangingTheLevelKeepsThePosition`: play two moves, change from Casual to Expert,
  assert the FEN, the move list, the side to move, and the stage.
- `TheNewLevelAnswers`: an engine that records the level it was asked for; after the
  change, the next request carries Expert.
- `ChangingTheLevelIsOfferedOnlyWhileAGameIsInProgress`: refused on the level list
  and after a finished game.
