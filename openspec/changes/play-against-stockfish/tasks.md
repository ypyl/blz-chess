# Tasks

Every task names how its completion is proven. The spec scenarios map to tests in
`tests/BlzChess.Core.Tests`, one test name per scenario title.

## 1. Rules in `BlzChess.Core`

- [ ] 1.1 `Position`: placement, side to move, castling rights, en passant, clocks,
      FEN parse and write. Verified by round-tripping the start position and
      Kiwipete through FEN unchanged.
- [ ] 1.2 `MoveGenerator`: sliding, knight, pawn (double push, en passant,
      promotion), king, castling through attacked squares, legality filter for
      pins and checks. Verified by perft 1-4 on the start position
      (20, 400, 8902, 197281), perft 1-2 on Kiwipete (48, 2039), and a
      castling/en-passant position at depth 3.
- [ ] 1.3 `Game`: apply and undo a move, move history, position repetition count,
      halfmove clock, and outcome detection for checkmate, stalemate, threefold,
      fifty-move, and insufficient material. Verified by one test per outcome,
      each built from a fixed position.
- [ ] 1.4 Scenario tests, names matching the spec: "an illegal move is refused",
      "an illegal move leaves the king in check", "a pawn reaching the last rank
      becomes a queen", "checkmate ends the game". Verified by `dotnet test`
      listing each name.

## 2. Level ladder and session

- [ ] 2.1 `Level` and the six-level table, with a seed-carrying random pick of a
      legal move. Verified by a monotonicity test over the table and by
      "the weakest level often misses the best move" with pinned seeds.
- [ ] 2.2 `GameSession` over `IEngine`, player White, illegal move refused, reply
      requested after each player move. Verified by session tests with a fake
      engine, including "the strongest level plays the engine choice".
- [ ] 2.3 Outcome and reset: "a new game asks for a level again". Verified by a
      session test.

## 3. Engine

- [ ] 3.1 Vendor `stockfish-19-lite-single.{js,wasm}` into `wwwroot/engine` from
      `stockfish@19.0.1`, with version and sha256 in `EngineInfo.cs`. Verified by
      `sha256sum` against the recorded constant and by a test that the file exists
      at the recorded path.
- [ ] 3.2 `wwwroot/engine/engine.js`: Worker, UCI handshake, `setoption` for skill,
      `position fen`, `go depth|movetime`, one promise per `bestmove`, and a
      cancel path. Verified in the browser smoke: Expert answers a move, and a
      second move does not race the first.
- [ ] 3.3 `StockfishEngine : IEngine` over `IJSRuntime`, loaded lazily on the first
      computer move. Verified by the browser smoke showing the board usable before
      the engine loads, and by the network panel fetching the wasm once.

## 4. The page

- [ ] 4.1 One route with the three states, the level list in spec order, the board,
      click-to-move, a thinking state that refuses input, the result line, and New
      game. Verified by "the page opens on the level list" and "choosing a level
      starts a game" in the browser smoke.
- [ ] 4.2 Pieces from the Cburnett set in `wwwroot/pieces`, with attribution.
      Verified by all twelve files present and named in `DESIGN.md` and `README.md`.
- [ ] 4.3 `DESIGN.md` with the palette, board proportions, and piece provenance.
      Verified by reading it against the built page.
- [ ] 4.4 Text and layout for a phone as well as a desktop. Verified by the smoke
      at 390x844 and 1280x800.

## 5. Docs and pipeline

- [ ] 5.1 `ARCHITECTURE.md`: the engine container is real, not planned. Verified by
      the diagram matching `wwwroot/engine` and the pass in `scripts/check-change.sh`.
- [ ] 5.2 `README.md` and `CHANGELOG.md`: what a player can now do. Verified by the
      changelog entry appearing under `## Unreleased`.
- [ ] 5.3 Deploy smoke extended to prove the engine asset is served (the deploy job
      already proves the shell and boot script). Verified by the hosted smoke job
      passing with the new check.

## 6. Verify and land

- [ ] 6.1 `bash scripts/verify.sh` green. Verified by its exit status.
- [ ] 6.2 `node scripts/gate.mjs play-against-stockfish --verify` matches every
      recorded hash. Verified by exit 0.
- [ ] 6.3 `review-change` run, including the security checklist for the vendored
      wasm and the worker boundary. Verified by findings resolved or recorded.
- [ ] 6.4 Archive, commit `feat` and `chore: archive`, push, watch `ci`, `deploy`,
      and `smoke` green, then merge. Verified by `gh run watch`.
