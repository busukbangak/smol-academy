## Description
Implemented MOBA/RTS mechanics for the planned multiplayer game smol academy (see smol-academy-GDD.pdf) made in Godot with C#. The game can be tested with the attached exe (exported game folder) or can be seen in the gameplay video (prototype-gameplay.mp4). The Goal is to defeat the enemy turrets and nexus to win the game. The assets were taken from the internet as placeholders. Those are free to use.

## Preview
![preview]gameplay.gif)

## Code
All Interactable Objects that can be targeted by the mouse are an Entity. The Entity base class consists of the base stats like attackDamage and movementSpeed and the assigned Team (See the attached diagram for more information: game-structure.pdf).

## Controls
* Mouse for moving and attacking and camera zoom
* Q/W/E/R for Abilities (Everything currently set on Blink with no cooldown)

## Features
* Ability System (Currently only Blink/Teleporation, which can be seen at the end of the video)
* Moba movement (point and click)
* moba combat (point and click)
* dance and emotes
* Turrets
* Melee and ranged Minions
* Turrets
* Nexus
* HP Bars
