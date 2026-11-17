# Stop the pieces flashing their alt text

L0

## Why

Choosing a level showed the board with the words "white pawn" and "black knight" in
the squares for a moment before the piece images appeared. Every `<img>` carried an
alt text, and a browser paints alt text while the image is still loading.

The text was also wrong for a screen reader: the square's button already reads
"e2, white pawn", so the image's alt repeated it with nothing to add.

## What Changes

The piece images become decorative: `alt=""` and `aria-hidden="true"`, since the
button is the thing that describes the square. That alone removes the flash, because
there is no text to paint.

The twelve piece files are also preloaded with the page, so a board that appears a
second after load paints complete rather than filling in. They total about seven
kilobytes.

## Routing

0. **Observable behavior changes?** The board no longer shows placeholder text. No
   spec requirement moves; the labels a screen reader hears are unchanged.
1. **Undoable in one commit?** Yes: one component attribute and twelve link tags.
2. **A machine can prove it correct?** Yes, in both directions. With every piece
   request forced to 404 the board's text content is empty and all 32 images are
   still present; with normal serving the page fetches all twelve piece files before
   a game starts.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: a visual defect, provable with the network broken.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/Pages/Play.razor` and `src/BlzChess.Web/wwwroot/index.html`.
