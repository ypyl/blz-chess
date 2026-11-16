# Threat model

Static site, no server, no accounts, no stored user data. The assets worth
taking are the repository, the deploy pipeline, and the visitor's browser.

## Assets

- The repository and its `main` branch, which is what visitors receive.
- The Pages deployment and its URL, which is what the maintainer's name is on.
- The `GITHUB_TOKEN` in Actions, and the OIDC identity the Pages job exchanges.
- The visitor's browser: no personal data is collected, but a compromised bundle
  runs with whatever origin trust the site has.

## Entry points and trust boundaries

- Visitor to static site: any content served from Pages, trusted because the
  repository is trusted.
- Pull request to `main`: code from a contributor reaches the build before a
  human reads it.
- Workflow to Pages: an OIDC exchange grants deploy rights to whatever the
  workflow ran.
- NuGet to build: third-party packages execute at restore and build time.
- Browser storage: in-progress game state on the visitor's device, never sent
  anywhere.

## Threats

| # | Threat | Path | Likelihood | Impact | Mitigation |
|---|---|---|---|---|---|
| 1 | Malicious code merged and served | pull request to `main` | Low | High | `main` accepts only merged change branches; every change runs CI before merge; the deploy workflow deploys `main` only, never a PR ref |
| 2 | Fork reaches deploy rights | PR workflow to Pages | Low | High | deploy permissions live on the `main`-only workflow; PR runs get `contents: read` and no OIDC token |
| 3 | Compromised action or package | Actions or NuGet supply chain | Medium | High | actions pinned to a major tag, dependencies locked with `packages.lock.json`, locked-mode restore in CI, one reviewed bump per change |
| 4 | Visitor state leaks between users | browser storage | Low | Low | state stays in the visitor's own browser; nothing is transmitted, so there is nothing to cross |
| 5 | Site defaced through a dependency script | build-time script | Low | Medium | no npm toolchain, no post-install scripts, build runs on a hosted runner from a locked graph |
| 6 | Pipeline holds more permission than it needs | workflow permissions | Medium | Medium | `pages: write` and `id-token: write` on the deploy job only; everything else `contents: read` |
| 7 | Vendored engine wasm is not what it claims to be | third-party build inside the app | Low | High | vendored from one pinned package version, sha256 recorded beside the version constant, upgrade is a reviewed diff, never fetched from a CDN at runtime |
| 8 | Engine payload used as a deniable flood or a first-visit stall | visitor download | Low | Low | lite network only, lazy load after the UI is interactive, cached by the service worker; Pages absorbs bandwidth |

## Accepted risks

| Risk | Why accepted | Who accepted | Revisit when |
|---|---|---|---|
| Deploy trusts the runner that published, with no separate approval step | a solo pet project; the human merges each change by hand and CI already gates it | repository owner, at the bootstrap gate | a second person can merge to `main`, or the site serves anything but chess |
| GitHub Pages cannot set security headers (CSP, frame options) | the page is static, script-only from its own origin, and holds no secrets or session | repository owner, at the bootstrap gate | the app renders third-party content, or any credential enters the browser |
| No integrity pinning of action SHAs to a commit | major tags are readable and bumpable in one reviewable change | repository owner, at the bootstrap gate | a supply-chain incident touches an action this repo uses |
| No engine process isolation beyond the Worker | the Worker is same-origin code the maintainer vendored; a hostile wasm build there would be the same trust boundary as the app bundle | repository owner, at the bootstrap gate | the engine is ever loaded from a different origin, or a second wasm binary enters the page |
