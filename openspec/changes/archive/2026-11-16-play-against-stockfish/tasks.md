# Tasks

Every task names how its completion is proven. All checked; each spec scenario has
a test whose name is the scenario title (`DisplayName`), which is what
`scripts/check-change.sh` looks for once the spec is durable.

## 1. Rules in `BlzChess.Core`

- [x] 1.1 `Position`: placement, side to move, castling rights, en passant, clocks,
      FEN parse and write. Done; `FenRoundTripKeepsEveryField` round-trips four
      positions byte for byte.
- [x] 1.2 `MoveGenerator`: sliding, knight, pawn (double push, en passant,
      promotion), king, castling through attacked squares, legality filter for pins
      and checks. Done; perft matches 20 / 400 / 8902 / 197281 on the start
      position, 48 / 2039 on Kiwipete, 14 / 191 / 2812 on a pawn endgame, and the
      castling position counts 26.
- [x] 1.3 `Game`: apply, history, repetition count, halfmove clock, and outcome
      detection. Done; one test per outcome.
- [x] 1.4 Scenario tests. Done: the four scenario names are `DisplayName`s in
      `GameTests`.

## 2. Level ladder and session

- [x] 2.1 `Level` and the six-level table with a seeded random legal move. Done;
      `AStrongerLevelIsNeverWeakerOnAnySetting` and the weakest-level scenario pass.
- [x] 2.2 `GameSession` over `IEngine`. Done; `FakeEngine` keeps the suite
      browser-free, and the strongest-level scenario passes.
- [x] 2.3 Outcome and reset via `GameFlow`. Done: `ANewGameAsksForALevelAgain` and
      `ARefreshLosesTheGame`.

## 3. Engine

- [x] 3.1 Vendor `stockfish-19-lite-single.{js,wasm}` from `stockfish@19.0.1`. Done;
      the wasm is 1.8 MB and its sha256 is a constant in `StockfishEngine`, next to
      the path.
- [x] 3.2 `engine.js`: Worker, UCI handshake, skill option, `position fen`,
      `go depth|movetime`, one promise per `bestmove`, serialized requests. Done; a
      real game in the browser fetched the wasm and answered at Expert.
- [x] 3.3 `StockfishEngine : IEngine` over `IJSRuntime`, imported on first use.
      Done; the board is interactive before the import, and the wasm request appears
      once in the network log.

## 4. The page

- [x] 4.1 One route with the three states, the level list in spec order, the board,
      click-to-move, a thinking state that refuses input, the result line, and New
      game. Done, and the page replaced the template's sidebar layout; the unused
      Bootstrap copy under `wwwroot/lib` went with it.
- [x] 4.2 Pieces from the Cburnett set. Done; twelve files under
      `wwwroot/pieces`, fetched from the Lichess piece sets, attributed in
      `README.md` and `DESIGN.md`.
- [x] 4.3 `DESIGN.md`. Done.
- [x] 4.4 Phone and desktop layout. Done: the board is 374px wide at 390px and
      34rem is the desktop cap.

## 5. Docs and pipeline

- [x] 5.1 `ARCHITECTURE.md`: the engine is a real container with a path. Done.
- [x] 5.2 `README.md` and `CHANGELOG.md`. Done.
- [x] 5.3 Deploy smoke proves the engine asset is served. Done: the smoke job
      fetches one byte of `engine/stockfish-19-lite-single.wasm`.

## 6. Verify and land

- [x] 6.1 `bash scripts/verify.sh` green. Done.
- [x] 6.2 `node scripts/gate.mjs play-against-stockfish --verify` exits 0. Done.
- [x] 6.3 `review-change` run, security checklist included for the vendored wasm.
      Done; findings are in the archive commit message.
- [ ] 6.4 Archive, commit `feat` and `chore: archive`, push, watch `ci`, `deploy`,
      and `smoke` green, then merge.
