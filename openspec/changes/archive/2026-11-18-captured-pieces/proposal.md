# Show the pieces that have been taken

L1

## Why

The board shows what is left, and nothing shows what has gone. A player who has just
lost a knight sees a square where it was and has to remember it. The count of material
is the one piece of information every chess player reads at a glance, and a casual
player reads it as "who is winning" without knowing that word for it.

## What Changes

Each side gets a tray beside the board. Pieces the computer has taken appear above the
board, nearest the pieces it still has; pieces the player has taken appear below,
nearest the player's own. A tray holds nothing at the opening and fills as the game
goes.

The tray is drawn at a reserved height on every screen, whether or not it holds
anything, so a capture cannot move the board. The loading screen reserves the same
height for the same reason: the board must be in the same place before and after the
app takes over.

Taking a move back puts the piece back on the board and out of the tray, and a piece
taken in passing shows up even though nothing moved onto its square. A pawn that had
become a queen and was then taken shows as a queen, not as the pawn it started life
as.

## Routing

0. **Observable behavior changes?** Yes. The page shows information it does not show
   today, and the board is one line smaller because the reserved tray height comes out
   of it.
1. **Can it be undone easily?** Yes: revert one commit, and the reservation with it.
   No data, no stored state, no contract.
2. **Can a machine prove it correct?** Yes. The record of what was taken lives in
   `Game`, beside the moves, so it is rebuilt by the same replay a take-back already
   uses: unit tests cover a capture, an en passant capture, a take-back, and a captured
   promoted queen. The claim that the board does not move is measured in a browser, by
   comparing the board's box before and after a capture and across the loading
   takeover.
3. **Does anything outside depend on it?** No. The spec is the only contract, and this
   change extends it.

Level **L1**: observable behavior changes and a machine can prove it.

## Capabilities

`game`. A new requirement, "The page shows the pieces each side has taken", with five
scenarios.

## Impact

`Game` records what was taken as it applies each move, because the alternative, counting
pieces on the board and comparing with the opening, cannot tell a captured queen from a
pawn that promoted into one. The page gains two rows, the stylesheet a fixed reservation
for them, and the loading markup the same reservation so the takeover stays still.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-18
- Artifact: openspec/changes/captured-pieces/specs/game/spec.md sha256:869ab8aa505822f6556e4107923440ea9fb563a9031d8b5ffab90c28c53a6553
- Artifact: openspec/changes/captured-pieces/proposal.md sha256:c573bb22c822469ccfc6af33739488e9eb7793742220c4ecb5951717cb231577
- Words: "(none)"
