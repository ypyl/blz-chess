# Tasks

## 1. Measure, fix, guard

- [x] 1.1 Measure the live site at phone sizes. Done: portrait fitted but the
      two-row header was not applying, and landscape scrolled 272 pixels.
- [x] 1.2 Remove the stray brace that killed the phone media block. Done, and a
      brace-depth pass over the stylesheet ends at zero.
- [x] 1.3 Cap the board and the loading board by the viewport height. Done:
      `scrollHeight` equals `innerHeight` at every size tested, including 844x390
      and 915x412, with the desktop board still 544.
- [x] 1.4 Validate installability against what a browser asks for. Done: manifest
      served as `application/manifest+json`, 192, 512, and SVG icons all 200,
      service worker active with a fetch handler.
- [x] 1.5 Add the guards. Done: the installability check in `verify.sh`, and the
      512 icon in the deploy smoke.
- [x] 1.6 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
