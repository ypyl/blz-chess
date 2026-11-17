# Give the app its own icon

L0

## Why

The tab, the install prompt, and the home screen all show the Blazor logo, because
that is what the template shipped and nobody replaced it. A chess app whose icon is
a framework's logo looks like a demo, and the manifest still called the install
background white while the app is dark.

## What Changes

One mark, drawn once as `wwwroot/icon.svg`: the Cburnett knight in cream on the
board's green, filling the tile so it still reads in a 16-pixel tab. The PNGs are
rendered from that file: 32 for the tab, 180 for apple-touch, 192 and 512 for the
manifest. The Blazor `favicon.png` goes.

The manifest now carries the three sizes, an SVG entry, the app's own colours
(`#1f1c18` for theme and background), and the page links the same set.

The deploy smoke fetches `icon.svg` and `icon-192.png`, because an icon that 404s
is the first thing a browser asks for and the last thing anyone notices.

## Routing

0. **Observable behavior changes?** The app looks like itself in a tab and on a
   home screen. No spec requirement moves.
1. **Undoable in one commit?** Yes: five asset files, one manifest, one markup line.
2. **A machine can prove it correct?** Yes: every icon answers 200 at its path,
   each PNG reports its declared size, the manifest parses with the same list, and
   the deploy smoke fetches two of them.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: assets and markup, no behaviour, fully reversible.

## Capabilities

None. No requirement moves.

## Impact

`wwwroot/icon.svg` (new source), `icon-32/180/192/512.png` (rendered), the removal of
`favicon.png`, `manifest.webmanifest`, `index.html`, `DESIGN.md`, `CHANGELOG.md`, and
one step of `.github/workflows/deploy.yml`.
