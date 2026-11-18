# Tasks

## 1. The coach and the switch, in Core

- [x] 1.1 `Level.Coach`: skill 20, depth 12, 600 ms, no randomness, not in the level
      list.
- [x] 1.2 `GameFlow.Hints`, `GameFlow.Suggestion`, and the toggle that asks the engine
      when it goes on.
- [x] 1.3 A token per request, so an answer that arrives after the player moved, after a
      take-back, or after the switch went off is dropped instead of drawn.
- [x] 1.4 Ask again after the computer's answer, and after a new game.
- [x] 1.5 Tests: off at the start, asked at full strength while the weakest level is
      playing, always a legal move, cleared when switched off, nothing about the game
      changed, refreshed after the computer answers, absent once the game is over.

## 2. The drawing

- [x] 2.1 A second arrow overlay for the suggestion: same geometry, dashed, thinner,
      dimmer.
- [x] 2.2 The header switch, `aria-pressed`, out of the way of Undo and Restart.
- [x] 2.3 A visually hidden line naming the suggested move.

## 3. Prove it in a browser

- [x] 3.1 Playwright: switch on, an arrow appears; the computer's arrow and the
      suggestion are both on the page and are drawn differently.
- [x] 3.2 Playwright: play the suggested move, the game accepts it.
- [x] 3.3 Playwright: switch off, the arrow goes and the board is unchanged.
- [x] 3.4 Playwright: 390x844, the header still holds level, Undo, Restart, Hints with
      no scroll.
- [x] 3.5 Screenshots: suggestion on, suggestion off, phone.

## 4. Verify and land

- [x] 4.1 `bash scripts/verify.sh`, with the commands and results recorded.
- [x] 4.2 `DESIGN.md` gains the second arrow and the switch; `CHANGELOG.md` the
      user-facing line.
- [x] 4.3 `feat`, `chore: archive`, merge `--no-ff`, push, `ci`, `deploy`, `smoke` green.

## Results

- `bash scripts/verify.sh`: lint and build clean, 85 tests pass.
- Playwright at 1280x800: the switch starts `aria-pressed="false"` with no arrow. Switched
  on, a dashed arrow appeared `e2-e4` (`450,650 -> 450,502`, `stroke-dasharray: 26px 20px`,
  `stroke-width: 9px`) and the hidden line read "Suggested move: e2 to e4.". After
  1.e4 the computer's solid arrow and the hint's dashed arrow were both on the page, and
  the hidden line followed the position: "Suggested move: b1 to c3.".
- Playing the suggested move was accepted by the game (b1 to c3, knight on c3).
  Switching hints off removed the dashed arrow, left the computer's arrow, and emptied
  the hidden line; the pieces on the board did not change.
- The arrow takes no clicks (`pointer-events: none`), and toggling the switch left the
  board's top at 234 on a phone and 544 wide on a desktop: nothing moves.
- 390x844: the header holds level, Undo, Hints, and Restart in 40px with no overflow,
  board 374, no scroll (844/844).
- Screenshots: `27-hint-and-engine-arrow`, `28-hint-phone`, `29-hint-desktop`.
