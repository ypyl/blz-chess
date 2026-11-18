# Start playing on load, with the level as a control

L1

## Why

The app opens on a level list and waits. Nobody needs that step: the level is already
a control in the header and can be changed at any time, so the screen exists to ask a
question whose answer the page can hold and change at will. It also costs a click to
every visitor and a second place where the game can be "not started".

## What Changes

The app starts a game as soon as it loads, at the weakest level, with White to move.
The level list screen goes, and with it the "back to the level list" ending: "New
game" after a result starts a fresh game at the level the header shows, so a player
who wants a different opponent changes it there, at any time.

The header keeps its shape: level, status, one action. The level is enabled whenever a
game is shown, including after a result, because the next game uses it.

## Routing

0. **Observable behavior changes?** Yes. The app opens on a board instead of a list,
   and "New game" starts a game instead of returning to a list.
1. **Undoable in one commit?** Yes. It removes a stage from the flow, a block from the
   page, and rewrites three requirements.
2. **A machine can prove it correct?** Yes: flow tests assert the stage, the level,
   and the position on load, after a restart, and after a new game; the browser check
   reads the board on a cold load.
3. **Does anything outside depend on it?** `openspec/specs/game/spec.md`. One
   requirement is removed, one is replaced, two are amended.

Level **L1**: the product's observable behavior changes, so the delta spec is the gate.

## Capabilities

**Modified** `game`: the level-list requirement is removed, the result requirement is
replaced by one that starts a new game at the level shown, and the restart and privacy
requirements are amended to the new ending.

## Impact

`BlzChess.Core`: `GameStage` loses `Choosing`, `GameFlow` starts a game in its
constructor and grows `NewGame` at the current level. `Pages/Play.razor`: the chooser
block and the `ChooseLevel` method go, and the level list is always enabled.
`DESIGN.md`, `CHANGELOG.md`, and the spec follow.

## Out of scope

Remembering the last level across a refresh, a first-run tour, and changing the
default level. The weakest level stays the opening opponent because the app cannot
know who is arriving.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-17
- Artifact: openspec/changes/auto-start/specs/game/spec.md sha256:6513a82bf2940c70d3c2691df1908a07ee6c031b07e3276571de656cac1a980b
- Artifact: openspec/changes/auto-start/proposal.md sha256:97eb28bbc6d2ebb52a041fa9c7853cf652a95db0595772a3d2715258b18bc0f9
- Words: "(none)"
