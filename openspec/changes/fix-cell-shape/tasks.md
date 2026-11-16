# Tasks

## 1. Measure, then fix

- [x] 1.1 Capture the failing measurement. Done: cell heights differed by row
      before the fix, because empty rows collapsed.
- [x] 1.2 Give both axes an explicit template of equal fractions, and stop images
      from sizing their track. Done.
- [x] 1.3 Remove the separator gap. Done, and `DESIGN.md` follows.
- [x] 1.4 Re-measure. Done: all 64 squares report `68.0x68.0`, and there is one
      distinct row height.
- [x] 1.5 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
