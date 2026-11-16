# Conventions

How the code is written here. One rule per heading, two lines each, and a rule
with no trigger is ignored. The ownership rule and how to add one are in
`AGENTS.md` under **Files**.

## Formatting and naming

`dotnet format --verify-no-changes` is the formatter, and `.editorconfig` is the
rule set. Never hand-format around it; change the editorconfig instead.

File-scoped namespaces, one public type per file named after it, `_camelCase` for
private fields, and no `this.`. A file ends with one newline and uses LF, whatever
the OS, because `.gitattributes` normalises to LF.

## Error handling and logging

The rules library rejects invalid input with `ArgumentException` and never
returns `null` to mean "illegal". A caller that has to check for null is a bug in
the library's contract.

No logging in `BlzChess.Core`: no console, no `ILogger`, no I/O. The web app logs
to the browser console through `ILogger`, and never logs a position, a move list,
or anything else it would be embarrassing to see in a screen share.

## Testing

xUnit, one test class per type under test, named `<Type>Tests`, in a project named
for the library it covers. Test names are sentences: `StartPositionPiecesAndMoveMarker`,
not `Test1`.

Every scenario in `openspec/specs` has a test whose name contains the scenario
title verbatim; `scripts/check-change.sh` fails when it does not. Rules get
property-style checks against known positions (perft counts) before they get
example tests.

## Dependencies

NuGet only. `BlzChess.Core` and the test project commit `packages.lock.json` and
restore with `--locked-mode` in CI. `BlzChess.Web` does not: the `wasm-tools`
workload injects SDK packs whose content hash differs between operating systems,
so its guard is the exact SDK pin plus explicit package versions. A new package is
an L2 change because it outlives the change that adds it.

Anything vendored as a binary (the Stockfish wasm) is pinned by version and by
sha256 recorded beside the version constant. Never loaded from a CDN at runtime,
so a repeat visit works offline and nobody's host stands between the user and the
asset.
