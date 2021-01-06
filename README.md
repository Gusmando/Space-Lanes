# Space Lanes

Space Lanes is a bullet-hell, lane-based shooter developed in the unity engine as a personal project in order to learn the ins and outs of game development within the engine and to gain experience within the Unity Scripting language, C#, as well as the variety of creation tools for asset creation.

### Tools

- All player and enemy sprite animation created in [Aseprite](https://www.aseprite.org/).
- UI Elements and other art created in [Adobe Illustrator](https://www.adobe.com/products/illustrator.html).
- 3d Modeling done in [Blender](https://www.blender.org/download/).
- Normal map's for sprites generated using [Cypetry's Normal Map Tool](https://cpetry.github.io/NormalMap-Online/).
- Real stars skybox taken from [Unity Asset Store](https://assetstore.unity.com/packages/3d/environments/sci-fi/real-stars-skybox-lite-116333).

### Features

- The game utilizes pre made stage sections and randomization to generate the stage at runtime.
- Game lighting indicates speed lanes(blue), low active lanes(green), and active lanes(red).
- Player has 5hp, getting hit by enemy results in 1hp loss.
- Player has control over lane and forward movement, the player cannot turn back.
- The player may jump for crossing gaps in platforms.

- #### Enemies
  4 Enemy Types
    - Minor Enemy (Blue enemy that simply rushes on the player and attempts to find their lane at random).
    - Projectile Enemy (Green enemy that avoids low active lane but keeps other lanes bullet-heavy).
    - Lobbing Enemy (Yellow Enemy that stands back from the player and shoots an arc of projectiles in rapid succession).
    - Boss Enemy (Orange enemy which is unreachable but has tells which indicate an open lane for player survival and vulnerability).
- #### Pickups
  3 Pickup Types
    - Health Pickup (Adds 1hp to the players current health, Does Nothing if full).
    - Double Jump (Upon pickup, timer will begin and ui indicator will note for how long the player may jump twice).
    - Spread Gun (The player's bullets will now shoot in three directions, but the ammo is limited running out will revert to original weapon).
- #### Weapons
  2 Weapon Types
    - Default (Shoots in a single direction, unlimited ammo, but faster fire rate).
    - Spread Gun (Shoots in 3 directions, lower fire rate, but clears enemies quickly).

### Post-Mortem

The process of creating Space lanes was a great opportunity for me to explore a solo project and experience all aspects of a game's development. In retrospect, learning the the unity library for scripting in C# was especially time consuming, but certainly rewarding in that it gave me an indication of what it is like to jump into a new language as well as an expansive library, truly showing me the massive amount of work that goes into game development. Additionally, the experience which I gained with such a wide variety of software is another reason why I love Game development, giving me an opportunity to utilize my creative intuition as well as my programming and aesthetic design sensibilities. Looking back I should have planned the systems in the game better programmatically, as often times adding new features would become cumbersome and complicated, but ultimately, there was also a lot to learn in that regard as my debugging skills and intuition for error detection increased over the course of development. Ultimately, it was a great experience and am excited to see what I can do in the future.

### Developer Notes

All assets that I created for this project can be found at my [opengameart account](https://opengameart.org/users/gusmando) and I encourage anyone to use them in their projects. Thanks for playing and make great games!
