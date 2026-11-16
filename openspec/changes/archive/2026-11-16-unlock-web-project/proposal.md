# Unlock the web project's restore so the runner can build it

L0

## Why

With the SDK pinned exactly, the third hosted run failed again, one step later:
NU1403, "package content hash validation failed for
Microsoft.NET.Sdk.WebAssembly.Pack.10.0.15". That pack comes from the
`wasm-tools` workload, and the Windows install and the Linux install serve
different bytes for the same package version. A lock file written on Windows
therefore cannot verify on the runner, and `--locked-mode` turns that difference
into a red build.

Locking the restore of a Blazor WebAssembly project is not achievable across
operating systems today. Pretending otherwise costs a red pipeline on every push.

## What Changes

`src/BlzChess.Web` sets `RestorePackagesWithLockFile=false` and loses its lock
file. `BlzChess.Core` and the test project keep theirs: they carry ordinary
packages, and their lock files are the part of the supply chain that can be
verified.

The guard that replaces the lock file for the web project is the exact SDK pin in
`global.json` plus explicit, reviewed package versions. A `shortcut:` marker in
the csproj names the ceiling and the condition for turning the lock file back on.

`CONVENTIONS.md` and the Toolchain fact say which projects are locked, so the next
change does not re-derive this the hard way.

## Routing

0. **Observable behavior changes?** No. Build and restore configuration only.
1. **Undoable in one commit?** Yes. One csproj property, one deleted file, two
   doc lines.
2. **A machine can prove it correct?** Yes, in both directions: the hosted run
   restores and builds, and the marker check in `scripts/check-change.sh` proves
   the deferred lock file carries an upgrade trigger.
3. **Does anything outside depend on it?** The Toolchain fact and the
   `CONVENTIONS.md` dependency rule, both updated here. The threat model in the
   bootstrap archive claims lock files mitigate supply chain risk; that claim is
   now true for two of three projects, and this proposal is the record of the
   narrowing.

Level **L0**, `skip_specs: true`. No gate.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/BlzChess.Web.csproj`, its deleted `packages.lock.json`,
`CONVENTIONS.md`, and the Toolchain fact in `AGENTS.md`. Nobody else builds this
repository.
