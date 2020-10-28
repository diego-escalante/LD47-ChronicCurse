# Chronic Curse Postmortem

## What Went Down

### Friday

_Brainstorming_
* I wrote down initial ideas under [notes.md](https://github.com/diego-escalante/LD47-ChronicCurse/blob/master/notes.md).
* I gravitated towards a 2D platformer where the player and other objects would loop back in time every so often.
* I Fleshed out the platformer idea, scoped out the work for the 48 hours, and made a prioritized list of tasks to do for the game (Core game features at the top, stretch goals at the bottom). 
    * This prioritized list method has worked well for me in past LD jams. It ensures that an MVP is done in time and all leftover time is spent iterating and improving on the MVP. I try to aim for completing the core game in half of the jam and leave the other half for extra features and polish.

_Platforming_
* I established basic 2D platforming by bringing in some base code I wrote beforehand. (This is allowed so long as you include it in the source code of your submission.)
    * This is a 2D platformer raycast-based system I've been working on. I turned off most of its features (like wall jumping, wall sliding, and multijumping) because these would distract from the main mechanic of the game, and would increase my amount of time drawing and animating the player performing these various actions (not my strong suit).
    * The end result was a 2D platformer that only has basic movement and jumping, but that hopefully *felt* good to play with. It includes things like coyote time and jump buffering.

_Looping_
* Created the core game mechanic; the looping. An ILooping interface and a LoopController made it very easy to get rolling quickly. Anything could implement ILooping to define what happens when it loops and would automatically subscribe to the LoopController, which was the main clock of the game. It would (eventually) keep time in sync with the music and tell all subscribers when to Loop().

_UI_
* Looping worked well but without any feedback it just felt like the game was buggy and stuck.
* Therefore I prioritized UI work to add a loop duration indicator for the player.
* I also added the controls of the game directly into the UI.
    * Unlike my previous LD jams, I wanted to make my controls as simple and as clear as possible.
    * This was inspired by the excellent Risk of Rain 2, which displays the button inputs next to the player's abilities.

_Goals and Levels_
* I created level goals and level transitions.
* With this, the core game loop is complete. The player can traverse an arbitrary number of (empty, noninteresting) levels. Time to sleep.

### Saturday

_Dialogue Boxes_
* I decided to round out the game with music, visuals, sounds and dialogue boxes before adding other level objects.
    * I could work on creating objects for the game until time was up, but end up with a game lacking art, music and the like. So like stretch goals, I decided to just leave that to the end to work on objects until time ran out.
* I created dialogue boxes to be able to loosely establish a setting for the game, and to explain the mechanic of the game.
    * In my past LD jams, I have left this stuff to the end and forced to quickly just put up a wall of text for the player to read at the start. I don't think this is a good player experience so I tried to tackle this differently this time around.

_Sprites and Animation_
* I turned my focus to visuals. I created pixel art with limited colors for the game with Aesprite.
    * Pixel art helps me combat my weakness in art skills. I find the low-resolution and few color options easier to work with by reducing the problem space.
* I chose yellow and purple as the primary colors of the game. These were used to implicitly tell the player what things would periodically loop (yellow), and what wouldn't (purple).
* Added a "ghost" to the player that indicates their starting position in the loop.

_Music_
* Used a [PO-20](https://www.youtube.com/watch?v=W5PvXQq3DVQ) and Audacity to create the music.
* Recorded two versions of the music: one with only a few "instruments" and one with all.
* Made the game music dynamic by hooking the two tracks up to the loop breaking mechanic and switching between them.
    * This helped hide any repetitiveness in my music but also felt really good when playing the game.
* Also synced up the loop cycles to the beat of the music. The music was 120 bpm, so having loops last 4 seconds made things sync up nicely.
    * Instead of the LoopController keeping its own 4 second clock, I made it poll for the music's current duration to better keep things synced.

_Loop Transitions_
* I decided to focus on improving the loop transitions.
    * During playtesting, loop transitions felt jarring and disorienting. I wanted to make this a smoother experience for the player.
* I made the player character stay in the same position of the screen during transitions so the player's eyes wouldn't have to repeatedly scan around the level to find the character.
* I made everything (except the player character) fade to black during loop transitions so that when the camera moved to its new position, all the level shuffling would happen outside the player's view to reduce disorientation. * With that, the game played well and was well rounded. All that as left was to make more objects to make the game more interesting and to build the levels. All of Sunday would be spent on this. Time to sleep.

### Sunday

_Objects_
* I set aside 2 hours at the end of the jam to build levels with whatever objects I would have ready by then. The rest was spent on object creation (spikes, doors, switches, keys, arrow dispensers).
* All of these objects were built relatively quickly since they were mostly isolated from one another.

_Levels_
* I created all the levels in the game, trying to use all the objects in interesting ways that would explore the main looping mechanic. Time's up. ggwp.

## What Went Well
* **Music**: Many players commented positively on the music about how it was catchy and dynamic. Indeed Audio was the highest rated category on my game, ranking 67th out of 800 game submissions. This was absolutely my favorite part of the game. I want to do more dynamic music for future jams.

* **Responsive Controls**: I was pretty happy to know other players liked the platforming and how they though the controls were tight and responsive. A bit of validation for this base platformer code I've been working on.

* **Polish**: A few players commented on the quality of the game's polish. With fading level and loop transitions, invincibility frames, dynamic music, animations, camera shakes, and dialogue boxes, I'm glad my efforts payed off.


## Want Went Wrong
* **Loss of Control**: This was mentioned a few times by other players, which I agree with. This is a flaw in game design. While I enjoyed coming up with and implementing the loop mechanic and trying something new, in actually I don't think the mechanic is very fun. The player loses too much control with the short automatic looping and this can feel frustrating. Another issue with this mechanic is that a lot of the time, any progress you are trying to make gets undone by the looping. So once you get a good grasp of the looping you tend to wait a while for your loop breaking power to regenerate, take some steps while the loop is broken, and then wait again doing nothing while you regenerate again. I think there might be a way to make this mechanic interesting and fun, but it's possible that a real-time-automatically-looping platformer is not the way.

* **Lack of Depth**: A few players mentioned that they wished that the game was a little more interesting and had more depth. I agree. Aside from the core mechanic, there's just not much in the game to make things interesting. Particularly, there's not many objects that loop (aside from the player) to explore the mechanic enough. A lot of the game is just platforming where you lose progress every 4 seconds. Additionally, the level design could have been better in order to better leverage the looping objects that _did_ exist in the game.

* **Creating Duplicate Sprites**: During the whole jam, I often manually created yellow and purple sprite versions of different objects (the player, doors, keys, etc). This was not necessary and I could have saved half that time by instead creating just one version of the sprites in grayscale, and dynamically coloring them yellow or purple in code depending on if they were looping or not. A case of working harder and not smarter.

* **Music Syncing Drift**: While the looping starts off in sync with the music, after a few replays of the music you start noticing that the looping is out of sync. While the LoopController explicitly uses the duration of the music to sync up, I think there is flawed logic here once the music replays and the apparent "duration" of this replayed music. This is untested, just a hunch. But something to keep in mind for next time.

## What Is Next
* I don't want to spend any more effort on Chronic Curse. It's an alright prototype, but not fun or promising enough without changing the game completely.
* However, I do want to refactor, generalize, and pack up some code which I think would be useful for future jams:
    * Dialogue manager, boxes, and triggers.
    * Music controller and syncing.
    * Scene transition and controller.
    * Some class extensions I wrote.
    * Camera Shakes.
* Any tweaks I had to do on the platformer system should also be captured and integrated to my platforming project.
* Maybe play around with 3D? I want to try something new for the next Ludum Dare.
