# Use shorter names for the levels

L1

## Why

The level list is as wide as its longest entry, "No idea how to play", which is eighteen
characters. On a desktop the dropdown takes 188 pixels of a 544-pixel column; on a phone
it is squeezed to 141 and takes the room the buttons need. The name of the weakest level
is the widest thing in the header, and a dropdown is not where a sentence belongs.

## What Changes

Two names shrink and one loses a word:

| Played today | Played now |
|---|---|
| No idea how to play | Random |
| Just learning | Learner |
| Beginner | Beginner |
| Casual | Casual |
| Club player | Club |
| Expert | Expert |

"Random" is also the truer name: that level plays a random legal move four times out of
five, which is what the spec already says about it. The ladder, its order, its strengths,
and what each level does are untouched.

## Routing

0. **Observable behavior changes?** Yes, in the smallest way the product has: the page
   names the levels, and the durable spec quotes two of those names. A requirement that
   quotes a name has to change when the name does, or a tester looks for a level that no
   longer exists.
1. **Can it be undone easily?** Yes: one commit, no data, no contract outside the spec.
2. **Can a machine prove it correct?** Yes. The ladder's names are asserted in tests, the
   spec's scenario titles are unchanged and still carry a test each, and the dropdown's
   width in a browser is measured before and after.
3. **Does anything outside depend on it?** Only the durable spec, which this change
   updates, and `README.md`, which says the range in words.

Level **L1**: the page shows different words, and the spec's own text changes with them.

## Capabilities

`game`. Two requirements are re-stated with the new names: "The computer answers at the
chosen level" and "The app starts a game as soon as it loads". Every scenario title stays
as it is, and every scenario keeps its test.

## Impact

`Level.All` holds the names. The loading screen's placeholder carries the weakest name, so
it changes too, or the header would shift when the app takes over. `README.md` names the
range. No strength, depth, or timing changes: this is the words on the buttons.

## Approval

- Gate: L1 delta spec
- Approved by: Yauhen Pyl
- Approved at: 2026-11-18
- Artifact: openspec/changes/short-level-names/specs/game/spec.md sha256:39167fab1aa06cd213337f4fb382594b8ec9d24817ce8f22a3fc69c8070ec499
- Artifact: openspec/changes/short-level-names/proposal.md sha256:a7232d13d3b40d1f5dae0829ba740616079c0190148e733cf2097103c3d25949
- Words: "(none)"
