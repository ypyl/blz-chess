# Changelog

One entry per user-facing change, newest first, under `## Unreleased`. The rules
are in `AGENTS.md` under **Versions and changelog**.

## Unreleased

### Changed

- The app has its own icon, a knight on the board's green, instead of the Blazor
  logo the template shipped.

### Fixed

- Knights were drawn as kings, because the piece image was chosen by the first
  letter of the piece name and Knight and King share it.

### Added

- Play a game against Stockfish. The page opens on six named levels, from "No idea
  how to play" to "Expert", and choosing one starts a game where you are White.
  The rules refuse illegal moves, the game announces checkmate, stalemate,
  repetition, the fifty-move rule, and insufficient material, and "New game"
  returns to the level list.

- The app: a Blazor WebAssembly PWA shell that installs and serves a BlzChess
  page, with a C# chess rules library and a test project behind it.
- A pipeline that builds, tests, and publishes the site to GitHub Pages on every
  push to `main`.
