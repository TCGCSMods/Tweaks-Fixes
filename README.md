# Tweaks & Fixes

A collection of tweaks and fixes for **TCG Card Shop Simulator**.

This mod aims to make small changes to various parts of the game, improving or adjusting behaviour that could be more convenient.

## Current Features

### Customer Trade Screen

- When a customer wants to sell a card, your offer is automatically set to their asking price instead of `0`.
  <br>
  So you can quickly accept their offer if you like it without having to manually type it in.

### Card Opening

- Reduces the delay after opening a booster pack, allowing you to move onto the next pack more quickly.
  <br>
  The total card value animation is also shortened from the game's default `2` seconds to `1` second.

- When a newly collected card is present, the final reveal sound is played at a higher pitch.

### Auto Pack opener

- Speeds up how long auto pack openers take to process each pack.
  <br>
  By default, they are very incredibly slow, and are slower than normal pack openings.

- Auto pack openers continue processing during the overnight transition.
  <br>
  Instead of ignoring the passage of time when the next day begins, the elapsed overnight time is applied so multiple packs can be processed automatically.

### Player Spawn

- Spawns the player at the shop door instead of on the road.

### Crouch Height

- Changes how far the camera drops while crouching to make it a little more useful.
  <br> 
  The default game offset is -1.0, which makes it difficult to look at things from a nice perspective.

## Configuration

The mods configuration file is generated at first launch at `BepInEx/config/TweaksAndFixes.cfg`.
<br>
Changes require a game restart for them to take effect.

<details>
<summary>View default config</summary>

```ini
[AutoPackOpener]

## Speeds up how long auto pack openers take to process each pack.
# Setting type: Boolean
# Default value: true
Enabled = true

## Multiplies how long each auto pack opener takes to process a single pack. Original: 1.0
# Setting type: Single
# Default value: 0.25
PackOpenTimeMultiplier = 0.25

[Card Opening]

## Enables shortening the delay before you can proceed to the next pack after the final card reveal.
# Setting type: Boolean
# Default value: true
ShortenNextPackDelayEnabled = true

## Delay in seconds before input to continue is accepted after the final card reveal. Original: 1 second.
# Setting type: Single
# Default value: 0.5
BeforeNextPackDelay = 0.5

## Enables a higher-pitched final gift sound when the pack contains a new card.
# Setting type: Boolean
# Default value: true
NewCardPitchEnabled = true

## Additional pitch applied to the final gift sound when the pack contains a new card.
# Setting type: Single
# Default value: 0.15
NewCardPitchIncrease = 0.15

[Crouch Height]

## Overrides how far the camera drops down while crouching.
# Setting type: Boolean
# Default value: true
Enabled = true

## Local Y offset applied to the camera while crouching. The game's default is -1.0.
# Setting type: Single
# Default value: -0.6
CameraDropY = -0.6

[Player Spawn]

## Changes the players spawn location to the shop door instead of on the road.
# Setting type: Boolean
# Default value: true
Enabled = true
```

</details>