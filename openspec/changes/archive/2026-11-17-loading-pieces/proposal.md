# Set the board up while the app loads

L0

## Why

The loading screen fills squares with colour, which shows progress but not what is
loading. The pieces are the product: a board being set up says "chess is arriving"
more directly than a band of accent colour, and it ends on the exact position the
first game starts from.

## What Changes

The loading board keeps its sixty-four cream and green squares and loses the colour
fill. The thirty-two pieces take its place: white back rank first, then white pawns,
black pawns, and the black back rank. Each piece fades and scales in as its share of
the progress arrives, so the board fills up in setup order and finishes set.

The pieces are the same SVG files the board uses and that the page already preloads,
so the loading screen costs nothing extra to fetch.

## Routing

0. **Observable behavior changes?** Only what the wait looks like. No spec
   requirement moves.
1. **Undoable in one commit?** Yes: one markup block, one loop, one CSS block.
2. **A machine can prove it correct?** Yes: with the runtime stalled, a published
   percentage of `50%` placed sixteen of the thirty-two pieces on the correct
   squares and none of the empty ones.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: presentation only, fully reversible.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/index.html`, `src/BlzChess.Web/wwwroot/css/app.css`, and the
loading section of `DESIGN.md`.
