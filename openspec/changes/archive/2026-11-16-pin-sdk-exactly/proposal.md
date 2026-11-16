# Pin the SDK patch, not just the band

L0

## Why

The previous change pinned the SDK band with `rollForward: latestPatch` and moved
CI to install from `global.json`. The hosted run still failed with the same
NU1004: `latestPatch` rolls to the highest patch in the band, so the runner
picked the 10.0.404 SDK that ships preinstalled and resolved
`Microsoft.NET.ILLink.Tasks` 10.0.15, while the lock file records 10.0.13 from
SDK 10.0.402.

A band pin is not a pin when the version that matters is decided by a patch
level. `--locked-mode` is only meaningful if both machines run the same SDK.

## What Changes

`global.json` uses `rollForward: disable` with version `10.0.402`, so exactly that
SDK runs, locally and on the runner. An SDK upgrade becomes one reviewed commit
that bumps `global.json` and regenerates the lock files together, which is the
diff the lockfile policy exists to produce.

The Toolchain fact in `AGENTS.md` is corrected: it now says the exact patch is
pinned and why.

## Routing

0. **Observable behavior changes?** No. Build tooling only.
1. **Undoable in one commit?** Yes, one JSON file and one doc line.
2. **A machine can prove it correct?** Yes. The hosted `ci` and `deploy` runs
   either restore in locked mode or they do not.
3. **Does anything outside depend on it?** The Toolchain fact, updated here.
   Nothing else reads `global.json`.

Level **L0**, `skip_specs: true`. No gate.

## Capabilities

None. No requirement moves.

## Impact

`global.json` and the Toolchain line in `AGENTS.md`. Anyone building this repo
needs SDK 10.0.402 exactly, which `dotnet` reports by name if it is missing.

## Note

The previous change assumed a band pin was enough. It was not, and the hosted run
said so in one line. That is the machine acting as the reviewer it is supposed
to be.
