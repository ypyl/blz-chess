# Tasks

## 1. Fix and prove

- [x] 1.1 `skipWaiting` and `clients.claim` in the published worker. Done.
- [x] 1.2 Reload once on `controllerchange`, never on first install. Done.
- [x] 1.3 Base the asset list on the worker's own URL. Done.
- [x] 1.4 Two-build test on one origin. Done: build A registered
      `offline-cache-v6zE/zSL`; after replacing the served files with build B and
      one reload the client reported `controller: yes`, `waiting: none`,
      `caches: offline-cache-OFcpE4Zf`, and the marker from build B's stylesheet.
- [x] 1.5 `bash scripts/verify.sh`. Done.

## 2. Land

- [ ] 2.1 Commit `feat`, archive, commit `chore: archive`, merge, push, watch
      `ci`, `deploy`, and `smoke` green. Verified by `gh run watch`.
- [ ] 2.2 Confirm on the live site that the worker is active and in control.
