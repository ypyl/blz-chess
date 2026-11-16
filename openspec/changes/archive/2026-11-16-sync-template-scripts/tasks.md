# Tasks

## 1. Sync

- [x] 1.1 Copy the four files from the template. Verified by `sha256sum` on both
      sides reporting identical hashes.
- [x] 1.2 Run the gate page tests. Verified by `node --test scripts/gate.test.mjs`
      reporting 24 passing.
- [x] 1.3 Confirm the recorded approval still verifies under the new hashing code.
      Verified by `node scripts/gate.mjs bootstrap --verify` exiting 0.
- [x] 1.4 Run `bash scripts/verify.sh`. Verified by "verify: all checks passed".

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge to `main`.
- [ ] 2.2 Push `main` once the remote exists, so the first hosted run exercises
      the synced gate script and the pipeline together.
