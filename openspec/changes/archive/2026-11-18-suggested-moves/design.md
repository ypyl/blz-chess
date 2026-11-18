# Show a suggested move when the player asks for one

## The coach is not the opponent

`Level` gains one more entry, `Level.Coach`, used only for questions and never offered
in the level list: skill level 20, depth 12, 600 ms, no random chance. It is a `Level`
record so `IEngine` needs no new method: the hint is `BestMoveAsync(fen, Level.Coach)`,
the same call the opponent's move makes with its own settings.

Depth 12 rather than the Expert level's depth 16 because the answer is drawn while the
player waits with the board in front of them. 600 ms is the difference between a hint
that feels like help and one that feels like the app is broken.

## Where the decision lives

`GameFlow` grows:

- `bool Hints` — off, not persisted.
- `Move? Suggestion` — the arrow, or nothing.
- `Task ToggleHintsAsync()` — flips the switch; on flips to on, asks the engine, and
  leaves the answer as `Suggestion`; off clears it.
- `Task SuggestAsync()` — ask, discard stale answers, draw what arrives.

Staleness is the one subtle piece. The answer is a `Task` that can land after the player
has already moved, or after hints were switched off. Each request takes a token; an
answer whose token is no longer current is dropped without touching the board. Without
that, a slow request can paint an arrow for a position that no longer exists, which is
worse than no arrow.

`SuggestAsync` is called when hints are turned on, after the computer's answer lands, and
after a new game or a take-back. It returns immediately: the engine runs in the
background and the page redraws when the answer arrives, so a hint never blocks a move.

`GameFlow` asks the engine for a suggestion with the same rules it uses for a move,
which is what makes the tests possible: a `ScriptedEngine` records what it was asked and
answers anything it likes, and the assertions are about the switch, the strength, the
legality of the answer, and the position being untouched.

## Drawing it

The board already has an overlay for the computer's last move. The suggestion is a second
instance of the same overlay with its own class: same geometry, dashed, thinner, and
dimmer, so two arrows at once read as "it played there" and "you could play there". The
dashed line is the difference that survives a colour-blind eye and a printed screenshot,
which a colour change alone would not.

Both are children of the board and `pointer-events: none`. Nothing about the layout
changes: the switch lives in the header, which already reserves its row.

## The switch

A button in the header's action group, `aria-pressed`, labelled "Hints". The header is a
grid that keeps its height, so a third control cannot move the board; on a narrow screen
the group is checked in a browser at 390 px, because "Hints" next to "Undo" and "Restart"
is the tightest row on the page.

The suggestion is drawn, not written, so the page also carries a line for a screen
reader that names the suggested move, in the same shape as the trays' labels. It is
visually hidden, so it changes no layout.

## What is not built

No ranked list, no evaluation number, no "why this move", no limit of one hint per turn,
no hint while the computer is thinking, and no remembering of the switch. Each of those
is a decision bigger than a switch, and none of them is needed to help a beginner.
