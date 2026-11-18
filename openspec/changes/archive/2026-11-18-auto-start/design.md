# Design: no stage between loading and playing

## What disappears

`GameStage.Choosing` and everything that existed to serve it: the chooser markup, its
buttons, the CSS for them, and `GameFlow.ChooseLevel` as a public method. A stage that
exists only to not play is a stage with one more way to be in the wrong place.

`GameFlow` starts a game in its constructor at `Level.Weakest`, so the page has a
session the moment it renders. There is no state where a session is missing, which
removes the null checks the chooser kept alive.

## What the three actions mean now

| State | Header action | Effect |
|---|---|---|
| Playing | Restart | fresh board, same level, no result |
| Finished | New game | fresh board, level shown, no result |
| Any | Level list | the opponent from the next move on |

Restart stays refused once a game has finished, which is what the spec's third
scenario wants; New game is the property of a finished game. Both call the same
private start, so there is one place that knows what a fresh game is.

The level list is enabled after a result as well, because changing it then is how a
player picks the next opponent. Nothing in the spec forbids it; the requirement only
promises the control during a game.

## Why the spec needed a replaced requirement

"The game ends and states the result" carried the sentence "A New game action SHALL
return the page to the level list". That sentence is now false, and the scenario that
tested it described a screen that no longer exists. The requirement is removed and
re-added as "The game ends and names the result" with the same promises minus that
one, rather than edited in place, so the durable spec keeps a clean history of what
changed and why.

## Testing

- `TheAppStartsAGameOnLoad`: a new flow is `Playing`, at the weakest level, on the
  starting position, with no moves.
- `ANewGameStartsAgainAtTheSameLevel`: after fool's mate at Expert, `NewGame` returns
  the start position, keeps Expert, and clears the result.
- `ChangingTheLevelIsAllowedAfterTheGame`: the change applies to the next game.
- `RestartIsOfferedOnlyWhileAGameIsInProgress`: unchanged in meaning, driven through
  the flow to a real `Finished`.
- The browser: a cold load lands on a board with `No idea how to play` in the header.
