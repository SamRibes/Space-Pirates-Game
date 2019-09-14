# Space-Pirates-Game
A game that I made for my Games and Media course at uni. Some interesting files to look at are [Entities](https://github.com/SamRibes/Space-Pirates-Game/tree/master/Space%20Pirate%20Game/Entities) and [GameRoot](https://github.com/SamRibes/Space-Pirates-Game/tree/master/Space%20Pirate%20Game/GameRoot).
# Game mechanics
The player controls a space ship that auto fires bullets and the goal is to collect enemy drops and level up their ship until it gets to the second level and then the boss level.

## Enemies
The enemies are created off screen at random positions along the left and right edges of the screen. Then they move inwards to the opposite side of the screen, while firing bullets at the player. 
When an enemy dies it will either drop a health pack which heals the player, or it will drop a coin which the player collects to increase their progress to the next level. Different enemies will drop coins worth different amounts based on the type of enemy.

<img src= "https://github.com/SamRibes/Space-Pirates-Game/blob/master/Space%20Pirate%20Game/Content/Screenshot%20(3).png" width= "2000">

## Leveling up
When enough coins have been collected to level up, the player ship gets an increase in their fire rate and their maximum health is increased.

## Boss Level
The boss is a big enemy made up of 5 sections. Each section has it's own health and fires it's own bullets. The central section doesn't fire anything.


<img src= "https://github.com/SamRibes/Space-Pirates-Game/blob/master/Space%20Pirate%20Game/Content/Screenshot%20(4).png" width= "2000">
