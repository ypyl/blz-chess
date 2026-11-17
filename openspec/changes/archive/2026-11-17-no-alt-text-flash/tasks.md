# Tasks

## 1. Fix and prove

- [x] 1.1 Replace the piece alt text with an empty alt and `aria-hidden`, since the
      square's button carries the label. Done.
- [x] 1.2 Preload the twelve piece files with the page. Done.
- [x] 1.3 Prove the flash is gone: block every piece request and read the board's
      text. Done: text is empty, 32 images present, no placeholder shown.
- [x] 1.4 Prove the board still completes: read the resource timings before a game.
      Done: 12 piece requests before a level is chosen, and all 32 images report
      `naturalWidth > 0` once the board renders.
- [x] 1.5 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
