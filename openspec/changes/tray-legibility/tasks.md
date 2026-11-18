# Tasks

- [x] 1.1 Give the tray the light square colour, rounded, with the shell's width.
- [x] 1.2 Draw the pieces at full opacity with a soft shadow; no dimming.
- [x] 1.3 A taller shelf on a narrow screen, out of the same variable the board's
      height budget uses.
- [x] 1.4 A `verify.sh` check that the tray background reaches 4.5:1 against black.
- [x] 1.5 Measure the contrast in the browser and screenshot the phone view.
- [x] 1.6 `bash scripts/verify.sh`, then commit, archive, merge, and watch CI.

## Results

- `bash scripts/verify.sh`: lint and build clean, 74 tests pass, and the new shelf
  check reports "a black taken piece reaches 17.9:1 on #f0eed6".
- Browser, 1280x800: both trays compute `rgb(240, 238, 214)` with pieces at opacity 1.
  After two captures the trays read "The computer has taken: white pawn, white pawn"
  and "You have taken: black pawn, black pawn".
- 390x844: shelf 36px, piece 32px, board 374px, no scroll (844/844).
- The guard itself was wrong at first: `[#0-9a-fA-F]{6}` treats `#` as a class member
  and captured five hex digits, so it measured `#f0eed` and reported 16.9:1. The `#`
  is a literal outside the class now and the number is the real one.
