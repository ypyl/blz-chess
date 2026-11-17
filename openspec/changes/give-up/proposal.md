# Let the player restart the game in progress

L1

## Why

The only action in a running game is "New game", and it does more than its name: it
ends the game without a result and drops the player back to the level list. Someone
who wants a fresh board at the same level has to pick their opponent again, and
someone who has just lost the position has no way to start over cleanly.

## What Changes

The header carries one action whose meaning follows the state. While a game is in
progress it is "Restart": same level, starting position, White to move, no result
recorded. Once the game is over it is "New game", which returns to the level list as
it does today.

One slot, one action, so the layout still does not move when a game ends.

## Routing

0. **Observable behavior changes?** Yes. The mid-game action now keeps the level
   instead of leaving it, and starts a clean game.
1. **Undoable in one commit?** Yes. One flow method, one component, one button label.
2. **A machine can prove it correct?** Yes: `GameFlow` tests for the level being
   kept, the starting position, the empty move list, and the absence of a result;
   the browser check reads the button's label in the three states.
3. **Does anything outside depend on it?** `openspec/specs/game/spec.md`, which gains
   one requirement through this delta.

Level **L1**: behaviour moves, so the delta spec is the gate.

## Capabilities

**Modified** `game`: one requirement is added. No new result kind, because a restart
records no result.

## Impact

`BlzChess.Core/GameFlow.cs`: `Restart()` keeps the level and starts a fresh session.
`Pages/Play.razor`: the header button takes its label and action from the stage.
`DESIGN.md` and `CHANGELOG.md` follow. The spec gains one requirement.

## Out of scope

Giving up or resigning with a recorded loss, confirming before restarting, keeping a
move history across restarts, undo, and offering a draw. A restart throws the game
away, which is what it says.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-17
- Artifact: openspec/changes/give-up/specs/game/spec.md sha256:349d578eccb5b6ee61957281ebdb4131706c3a1494ec000a6232ba9f24b8b8b2
- Artifact: openspec/changes/give-up/proposal.md sha256:05dfd5a4fded065037c44f1a44ae95162a5af288ffab7e2c3e215325eb6ccd7a
- Words: "let's be it restart instead of give up"
