# Let a deploy reach the tab that is already open

L0

## Why

Every deploy after the first was invisible to a returning visitor. The stock
Blazor worker installs a new version only when it can, and it can only when every
tab of the site is closed: without `skipWaiting` it sits in `waiting`, and the old
shell keeps being served from the old cache. Clearing site data was the only way
to see the current build, which is not something to ask of a player.

That is how a board with four kings a side stayed on screen after the fix had
shipped.

## What Changes

The published worker calls `self.skipWaiting()` once its cache is filled and
`self.clients.claim()` on activate, so a new build takes over instead of queueing.

The page listens for `controllerchange` and reloads once when a worker it did not
start with takes control. On first install there is nothing to reload, so the
listener does nothing.

The worker's own URL is now the base for its asset list, instead of a hard-coded
`/`. The app is served from a sub-path, and the code already knew it.

## Routing

0. **Observable behavior changes?** A deploy now reaches an open tab on the next
   load. No spec requirement moves; `openspec/specs/game/spec.md` has nothing to
   say about updates.
1. **Undoable in one commit?** Yes: two files, both in `wwwroot`.
2. **A machine can prove it correct?** Yes, and it was proven twice over: with two
   builds served from one origin, one reload moved the client from
   `offline-cache-v6zE/zSL` to `offline-cache-OFcpE4Zf`, left no waiting worker,
   deleted the old cache, and served the new CSS from the new cache.
3. **Does anything outside depend on it?** No spec, no ADR, no contract document.

Level **L0**: a defect that restores the behaviour the README already assumes, and
the evidence is a cache key that moved.

## Capabilities

None. No requirement moves.

## Impact

`src/BlzChess.Web/wwwroot/service-worker.published.js` and
`src/BlzChess.Web/wwwroot/index.html`.

## Accepted trade-off

A reload in the middle of a game loses that game, because the spec already says a
refresh loses it. The alternative is a "new version, reload?" prompt that waits for
the game to end; that is a change of its own, and until then staying on a stale
build is worse than losing one game.
