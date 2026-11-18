# Turn the loading board the right way round

L0

## Why

The loading board had its two colours the wrong way round: its top-left square was
green where a real board has it cream, and its bottom-left was cream where a real
board has it green. Every square was inverted, so the board looked like a board seen
in a mirror, and then corrected itself the moment the game appeared.

The game board derives its colours from the square's file and rank. The loading board
is static markup written by hand, so it had its own copy of that rule, and the copy
was wrong.

## What Changes

The loading board's squares are regenerated with the rule the board uses: a square is
light when its file and rank differ in parity, which makes a8 cream and a1 green. A
comment points at the rule in the markup.

`scripts/verify.sh` gains the check that would have caught it: it reads the sixty-four
square classes out of the static markup, compares each with the parity a real board
has, and fails naming the square that disagrees. The check was run against a copy
with one square flipped and reported `FAIL square 0 is dark`, so it fails for the
right reason.

## Routing

0. **Observable behavior changes?** Only the loading board's colours. No spec
   requirement moves.
1. **Undoable in one commit?** Yes: one markup block and one check.
2. **A machine can prove it correct?** Yes: the guard compares all sixty-four squares
   with the real board's parity, and the browser reads `a8` as `rgb(240, 238, 214)` in
   both the loading view and the game.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: a visual defect with a machine check behind it.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/index.html`, `scripts/verify.sh`, and one line of
`DESIGN.md`.
