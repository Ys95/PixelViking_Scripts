# Links

[**Itch.io** (Playable in browser)](https://ys95.itch.io/pixelviking)

# Short gameplay

<iframe width="560" height="315" src="https://www.youtube.com/embed/tq1-kkjXB2k?controls=0" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

# Game features

## Inventory system

Player can collect variety of items. Each item has its own use - keys can be used to open treasure chests, consumables temporarily increase attack or defense and spells can be used to attack mobs. 

### Important classes:
* [ItemObject](https://github.com/Ys95/PixelViking_Scripts/blob/main/InventorySystem/ItemObject.cs) - Abstract class, all items derive from it. Contains basic data like item name, description or icon.
* [InventoryObject](https://github.com/Ys95/PixelViking_Scripts/blob/main/InventorySystem/InventoryObject.cs) - Contains array of owned items as well as methods for managing (adding, removing, saving) them. 
* [InventorySlotsDisplay](https://github.com/Ys95/PixelViking_Scripts/blob/main/UI/InventorySlotsDisplay.cs) - Handles displaying and managing inventory in the UI (dragging items to move them to other slots etc). 
* [LootTableObject](https://github.com/Ys95/PixelViking_Scripts/blob/main/InventorySystem/LootTableObject.cs) - Allows to create loot tables. Each loot table contains list of items with chances for their drop as well as methods for determining which item will drop. 
* [ConsumableItemsBuffs](https://github.com/Ys95/PixelViking_Scripts/blob/main/Player/ConsumableItemsBuffs.cs) - Manages temporary modifications of player stats (attack, defense, health regeneration) by items.

## Save system

Each saveable object contains 2 basic informations: object ID and world position. Depending on object type, addinational data might need to be stored, for example player needs to have his inventory saved. 

### Important classes:
* [SaveableObject](https://github.com/Ys95/PixelViking_Scripts/blob/main/SaveSystem/SaveableObject.cs) - Abstract class defining basic data that needs to be saved as well as methods for creating it.
* [SaveableObjectsManager](https://github.com/Ys95/PixelViking_Scripts/blob/main/StaticAndSingletons/SaveableObjectsManager.cs) - Singleton, used to delete/restore saveable objects.
* [SaveSystem](https://github.com/Ys95/PixelViking_Scripts/blob/main/StaticAndSingletons/SaveSystem.cs) - Singleton used to create and load save files.
* [PrefabsDatabase](https://github.com/Ys95/PixelViking_Scripts/blob/main/Databases/PrefabsDatabase.cs) - Used to fetch prefabs via their id so they can be instantiated when game gets loaded.

Example save file can be found [here.]()

## Enemies

Enemies have simple AI: They will patrol from point A to point B untill they detect player. What happens after player gets detected depends of enemy type - basic enemies will simply chase player until he leaves their detection range(trigger). Ranged enemies will immediately shoot player and will continue to do so as long as player remains in their line of sight(raycast). 
Pathfinding is handled by A* pathfinding plugin.

[Gifs]

### Important classes:
* [EnemyBehaviour](https://github.com/Ys95/PixelViking_Scripts/blob/main/Enemies/EnemyBehaviour.cs) - Defines how enemies behave and how they move, handles pathfinding.
* [RangedEnemyAttack](https://github.com/Ys95/PixelViking_Scripts/blob/main/Enemies/RangedEnemyAttack.cs) - Defines logic of ranged enemies - how they look for target, what they consider a target, their vision range etc.

##Other important classes

* [CharacterMovement](https://github.com/Ys95/PixelViking_Scripts/blob/main/Character/CharacterMovement.cs) - Used by enemies and players, defines how character will move.
* [ObjectPool](https://github.com/Ys95/PixelViking_Scripts/blob/main/StaticAndSingletons/ObjectPool.cs) - Implementation of object pooling. Important for classes that otherwise would end up instantiating and destroying a large amount of objects, for example [FloatingCombatText](https://github.com/Ys95/PixelViking_Scripts/blob/main/StaticAndSingletons/SoundManager.cs) or [SoundManager](https://github.com/Ys95/PixelViking_Scripts/blob/main/StaticAndSingletons/SoundManager.cs). 
* [PrefabMaker]() - Editor script used together with [PickupScript](https://github.com/Ys95/PixelViking_Scripts/tree/main/Environment) to automatise creating Pickups prefabs from item objects.

[Vid]






