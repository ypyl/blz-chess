# Tasks

## 1. Build and prove

- [x] 1.1 Replace the spinner markup with the mark over a rank. Done.
- [x] 1.2 Drive the rank and the number from the two properties Blazor sets, with
      reduced motion respected. Done.
- [x] 1.3 Check the rendering at a fixed percentage in a standalone page. Done: the
      rank fills, the partial cell is cut by the clip, and the number matches.
- [x] 1.4 Check the real page with framework requests delayed. Done: the document
      reported `1.4%` and the number read `1%` while the loading markup was still in
      place.
- [x] 1.5 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
