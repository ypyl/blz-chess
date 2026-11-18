# Use shorter names for the levels

## The names are data, the widths are not

The dropdown is `justify-self: start`, so its width is the intrinsic width of its longest
option plus padding. Nothing else sets it: shorten the names and the control narrows by
itself, on every screen, with no layout rule to keep in step. That is why this change
touches `Level.All` and no stylesheet.

The longest name becomes "Beginner" at eight characters, against "No idea how to play" at
eighteen, so the control loses roughly seventy pixels of intrinsic width. On a phone it is
already capped by its grid column, so what the player gets is the room the buttons were
short of, not a narrower control.

## Two places that must agree

The loading screen draws the header before Blazor runs, with a placeholder carrying the
weakest level's name. The placeholder is `visibility: hidden` and sits in the same grid
column, so its width is the column's, not its own; the words still have to change, because
a stale name in the markup is a name someone will read in the source and believe.

`README.md` says the range in words and names the two ends. It changes with the levels, not
after them.

## What does not change

Strength, depth, time, and random chance are the same numbers on the same rows. Level 1
still plays a random legal move four times in five; only the word for it is shorter, and
truer: "Random" describes that behaviour, and "No idea how to play" described a person.
