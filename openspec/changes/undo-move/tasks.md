# Tasks

## 1. Core

- [ ] 1.1 `Game.TrimMoves(int)`: drop the last N moves, rebuild the position and the
      repetition keys by replay. Verified by a restore test comparing FEN, moves, and
      the side to move.
- [ ] 1.2 `GameSession.UndoPlayerTurn()`: refused unless it is the player's turn, so
      the pair goes together. Verified by tests for the refused cases.
- [ ] 1.3 `GameFlow.UndoLastMove()`: refused unless a game is running. Verified by a
      flow test including a finished game.
- [ ] 1.4 The rebuilt repetition state is real. Verified by the threefold test that
      fails on a cosmetic rebuild.

## 2. Page

- [ ] 2.1 "Undo" beside "Restart" while playing, inert at the opening, and "New game"
      alone when the game is over. Verified in the browser in all three states.
- [ ] 2.2 The board does not move on any of it. Verified by measuring its top edge.

## 3. Docs

- [ ] 3.1 `CHANGELOG.md` and `DESIGN.md`. Verified by reading both.

## 4. Verify and land

- [ ] 4.1 Every scenario in the delta spec has a test whose name contains its title.
      Verified by `scripts/check-change.sh` after the archive.
- [ ] 4.2 `bash scripts/verify.sh`, `node scripts/gate.mjs undo-move --verify`,
      `review-change`.
- [ ] 4.3 Archive, commit, push, watch `ci`, `deploy`, `smoke`, merge.
