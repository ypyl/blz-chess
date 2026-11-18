# Tasks

## 1. The coach and the switch, in Core

- [ ] 1.1 `Level.Coach`: skill 20, depth 12, 600 ms, no randomness, not in the level
      list.
- [ ] 1.2 `GameFlow.Hints`, `GameFlow.Suggestion`, and the toggle that asks the engine
      when it goes on.
- [ ] 1.3 A token per request, so an answer that arrives after the player moved, after a
      take-back, or after the switch went off is dropped instead of drawn.
- [ ] 1.4 Ask again after the computer's answer, and after a new game.
- [ ] 1.5 Tests: off at the start, asked at full strength while the weakest level is
      playing, always a legal move, cleared when switched off, nothing about the game
      changed, refreshed after the computer answers, absent once the game is over.

## 2. The drawing

- [ ] 2.1 A second arrow overlay for the suggestion: same geometry, dashed, thinner,
      dimmer.
- [ ] 2.2 The header switch, `aria-pressed`, out of the way of Undo and Restart.
- [ ] 2.3 A visually hidden line naming the suggested move.

## 3. Prove it in a browser

- [ ] 3.1 Playwright: switch on, an arrow appears; the computer's arrow and the
      suggestion are both on the page and are drawn differently.
- [ ] 3.2 Playwright: play the suggested move, the game accepts it.
- [ ] 3.3 Playwright: switch off, the arrow goes and the board is unchanged.
- [ ] 3.4 Playwright: 390x844, the header still holds level, Undo, Restart, Hints with
      no scroll.
- [ ] 3.5 Screenshots: suggestion on, suggestion off, phone.

## 4. Verify and land

- [ ] 4.1 `bash scripts/verify.sh`, with the commands and results recorded.
- [ ] 4.2 `DESIGN.md` gains the second arrow and the switch; `CHANGELOG.md` the
      user-facing line.
- [ ] 4.3 `feat`, `chore: archive`, merge `--no-ff`, push, `ci`, `deploy`, `smoke` green.
