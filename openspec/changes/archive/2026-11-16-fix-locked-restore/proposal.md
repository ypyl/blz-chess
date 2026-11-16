# Pin the SDK so the locked restore holds

L0

## Why

The first hosted run failed on `dotnet restore --locked-mode` with NU1004:
`Microsoft.NET.ILLink.Tasks` requested `[10.0.15, )` instead of the `[10.0.13, )`
the lock file records. The `wasm-tools` workload injects that package, and its
version follows the SDK patch. The dev machine runs SDK 10.0.402, which resolves
10.0.13; the runner installed "latest 10.0.x" (10.0.404) and resolved 10.0.15.

The lock file is not wrong. Two machines resolving different toolchains is. Left
alone, this breaks every time GitHub ships a .NET patch, and the fix will always
look like a mystery NU1004.

## What Changes

`global.json` pins SDK `10.0.402` with `rollForward: latestPatch`, so the band is
fixed and patches inside it are still picked up. Both workflows install the SDK
`global.json` names instead of `10.0.x`. The Toolchain fact in `AGENTS.md` says
so, because it is now a rule the code depends on.

Package versions do not move: `dotnet restore --force-evaluate` against the
pinned SDK leaves every `packages.lock.json` byte-identical, which is the proof
that 10.0.402 is the toolchain those files were resolved on.

## Routing

0. **Observable behavior changes?** No. Build tooling only.
1. **Undoable in one commit?** Yes. One file, two workflow steps, one doc line.
2. **A machine can prove it correct?** Yes. `dotnet restore --locked-mode` plus
   the rest of the CI steps, run locally and then on the runner.
3. **Does anything outside depend on it?** The Toolchain fact in `AGENTS.md`,
   which this change updates. No spec, no ADR, no contract document.

Level **L0**, `skip_specs: true`. No gate: it is a red pipeline restored to
green, and the evidence is the run itself.

## Capabilities

None. No requirement moves.

## Impact

`global.json`, `.github/workflows/ci.yml`, `.github/workflows/deploy.yml`, and the
Toolchain line in `AGENTS.md`. Nobody else's build changes: the repository was
public for four minutes and has no contributors.
