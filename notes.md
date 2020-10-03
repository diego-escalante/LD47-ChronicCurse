# Notes
* Theme: Stuck in a loop

## Brainstorm
* ~~Factory that has cyclical processes and obstacles and you are forced to maneuver through it's looping cycles...~~

* ~~Some sort of dodging game that is memory based, things loop and over time more things are added to the loop. Kind of like an "action" simon says game.~~

* platformer where the player loops back ever X seconds back to where they were. Everything else (maybe not everything???) presists. Interesting combat and puzzles... time consuming.
    * There needs to be a mechanic for the player to not need to loop back every time. Maybe a power charges that allows the player to "skip" a loop. Thus changing the initial loop state.
        * recharging allows for action.
        * limited allows for puzzles.
    * A little shader to show "presisting" objects (Braid style) would be nice.
    * Each loop you snap back to position. Do you keep your momentum? Yeaaaaah.

* ~~Maybe a game where the point is not to work within a loop or break out of it, but rather _maintain_ a loop. That is, you know what the looping looks like, you must make sure it happens that way.~~
    * ~~It's almost like a self-fulfilling-prophecy-thingy.~~

* ~~Looping midi sounds/music that the player acts with? Rhythm game. I feel like a lot of people will go this route.~~

* ~~History repeats itself~~

* ~~What loops? Time, planets, calendars, water cycle.... bleh.~~

## Looping Platformer: Chronic Curse
* 2D platformer.

### Core Idea:
* The game has a time loop of X seconds. After X seconds, everything that is not "presistent", lets call them loopers, snap back to where they were at the start of the loop.
    * Momentum is conserved, Portal style.
* The player is a looper. It snaps back.
* Death to the player means nothing, they just loop back at the end of the loop.
* In order to make any progress, the player has ways to stop being a looper for limited time. That is, they can skip loops.
    * Thus, resetting their starting position in the loops.
    * This is a recharging power. I.E, every X loops, they have the ability to break out of 1 loop.
* Main objective: Traverse ~~one BIG level (metroidvania) or~~ many small levels. 
    * ~~1 Big level is epic. But much more work. Maybe not viable.~~
    * Small levels are more standard and unremarkable, but gives more control of the level design, and it's modular. Not bad for a jam. Fine. ok.
* What kind of story will the game have?
    * Generic human - breaks into an ancient temple. Turns out to be the temple of Chronos.
    * Cursed by Khronos to loop forever until the player escapes the temple.

### Other Components:
Things that explore the main mechanic.

* Other things can also be loopers. Doors, keys, platforms, etc.

* Doors and Keys: These map 1:1 (no Zelda style dungeon keys and doors). Get a door's key to unlock it.
    * Keys can be loopers. That is, use a "green" key to open one "green" door. It's used up but at the end of the loop it reappears, but the door stays open. So it can open multiple doors.
    * Maybe doors can loop too. Makes them 1-way. You can open them but they close behind you after a loop.

* Moving platforms: Standard platforms that move.
    * Can be loopers.

* Spikes: Kill on contact.

* Switches: Press to open a door at a distance.
    * Can be loopers. These are your standard timed switches, because when the loop ends they reset.

* Time Elementals: Generic enemies. Move back and forth. Kills on contact.
    * Can be loopers. This can snap them back to their starting position if they fall or something.
    * Got to be careful of not getting into an impossible situation where the player loops right on top of an enemy, which gets them stuck forever. Invincibility frames maybe?

* Turrets: Shoot "time energy" idk, dude. But they just shoot periodically. projectiles kill on sight.

* Other powers:
    * Increase loop length.
    * Add extra loop breaking charges.

That's it. No way I'm even getting through all that.

## TODO:
Core Game:
* ~~Standard 2D Platforming.~~
* Looping player.
* Loop breaking power and regeneration.
* UI: Show loop time, loop breaking power charge, controls.
* Basic levels - starting points, goals, transition between levels.
* Tutorial level (Player enters temple, no looping, just get used to standard platforming. _Maybe_ short dialogue to explain story.)

Extra Mechanics:
* Spikes and dying.
* Moving platforms.
* Doors and keys.
* Turrets.
* Switches.
* Elementals.
* Other powers.