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

### Player Spawn

- Spawns the player at the shop door instead of on the road.

## Configuration

The mods configuration file is generated at first launch at `BepInEx/config/TweaksAndFixes.cfg`.
<br>
Changes require a game restart for them to take effect.

<details>
<summary>View default config</summary>

```ini
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

[Player Spawn]

## Changes the players spawn location to the shop door instead of on the road.
# Setting type: Boolean
# Default value: true
Enabled = true
```

</details>