# Point the shell at the sub-path before publish, not after

L0

## Why

With the dotfile gone, the service worker still fails to install. The published
worker caches each asset as a `Request` carrying the hash the build recorded, and
`index.html` no longer matches its record: the deploy job rewrites the shell's base
href *after* publish, which changes the bytes of a file whose hash was fixed
earlier. `cache.addAll` rejects, the install goes `redundant`, and the app is still
not installable.

The served hash and the manifest agree on nothing:
`sha256-W8s4ngM0…` against `sha256-KJRv3qrN…`.

## What Changes

The base href is rewritten in the source tree before `dotnet publish`, inside the
CI checkout only. The published file is then the file the build hashed, and the
manifest is true by construction.

A new deploy step proves it every time: it recomputes the sha256 of every file in
the published service worker manifest and fails the run on the first disagreement,
missing file included. That is the check that would have caught this in a minute
instead of a browser console.

`404.html` is still written after publish, because it is a copy of the shell that
the worker never caches.

## Routing

0. **Observable behavior changes?** The app becomes installable, which the README
   and `adr/0001` already claim. No spec requirement moves.
1. **Undoable in one commit?** Yes: one moved step and one added step, both in the
   deploy workflow.
2. **A machine can prove it correct?** Yes. The hash check runs on every deploy,
   and the browser reports the worker reaching `activated`.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0** by `fix-bug`: reversible, provable, unshared.

## Capabilities

None. No requirement moves.

## Impact

`.github/workflows/deploy.yml` only. The repository's `index.html` keeps `/` as its
base so `dotnet run` still works.
