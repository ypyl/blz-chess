# Check for a new build when the app comes back into view

L0

## Why

An installed app is resumed, not reloaded. Android keeps the page alive for days, so the
registration script runs once, asks for a new build once, and then never again while the
build changes underneath. The result is a person looking at an old board in an app that
was updated yesterday, with nothing on screen to say so.

The asking works when it happens: with a new build served and a `visibilitychange`
dispatched, the worker installed, the cache moved from `offline-cache-FOREGROUND` to
`offline-cache-RESUME`, and the page reloaded onto it. Nothing asked at the moment a
person would notice.

## What Changes

The registration asks again whenever the page becomes visible, and on `pageshow`, so a
resumed app checks the same way a freshly opened one does. The reload that a new worker
triggers is unchanged, so a check that finds nothing costs one conditional request.

## Routing

0. **Observable behavior changes?** Only when: an app that has been in the background may
   reload itself onto a new build where before it stayed old until it was killed. Nothing
   about the game, the board, or the rules changes.
1. **Can it be undone easily?** Yes: two listeners, one commit.
2. **Can a machine prove it correct?** Yes, and it was: a local publish served as two
   builds, with the second one live and a `visibilitychange` dispatched, moved the cache
   and reloaded the page onto it.
3. **Does anything outside depend on it?** No.

Level **L0**: a listener on an existing mechanism, reversible, proven by the update it
causes.

## Consequence worth naming

A reload loses the game in progress, and that was already true of the update path: a new
worker reloads the page. This change makes that happen on resume as well, which is the
only way an app that is never killed can take a new build.
