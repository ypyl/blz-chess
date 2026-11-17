# Tasks

## 1. Draw and wire

- [x] 1.1 Draw `icon.svg` from the piece set the board already uses. Verified by
      rendering it at 512 and 32 and reading both.
- [x] 1.2 Render the PNGs from that file at 32, 180, 192, and 512. Verified by
      `file` reporting the exact pixel size of each.
- [x] 1.3 Point the manifest and the page at them, and delete `favicon.png`.
      Verified by reading the served page's icon links and the manifest.
- [x] 1.4 Add the smoke step. Verified by the next deploy's smoke job.
- [x] 1.5 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
- [ ] 2.2 Confirm the deployed icons. Verified by fetching each path and reading the
      live links.
