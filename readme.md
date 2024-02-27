
# AiDisabler Mod for Sons Of The Forest

This mod allows players to disable specific AI entities from spawning in the game by instantly killing them when they spawn (so you can still collect their resources) or deleting them entirely. For example, if you don't want to play with mutants/creepies, you can disable them, or if you just don't like the mutant babies, you could configure them to not spawn.

## Installation

1. **Download**: Download the latest release of the AiDisabler mod from the [Releases](https://github.com/LexTheGreat/AiDisabler/releases) page.
2. **BepInEx**: Download the latest BepInExPack IL2CPP from [Thunderstore](https://thunderstore.io/c/sons-of-the-forest/p/BepInEx/BepInExPack_IL2CPP/).
3. **Extract BepInEx**: Extract the contents of the downloaded BepInEx ZIP file into your game folder.
4. **Run the game once**: Run the game once with BepInEx installed. When you get to the main menu, exit the game.
5. **Extract AiDisabler**: Place AiDisabler.dll from the AiDisabler.zip file into your plugin folder (create it if it doesn't exist: bepinex/plugin).
6. **Extract AiDisabler Config**: Place AiDisabler.json/AiDisabler.cfg from the AiDisabler.zip in your config folder (create it if it doesn't exist: bepinex/config).
7. **Configure AiDisabler**: Edit the AiDisabler.json file to change which AIs are controlled by AiDisabler. Examples of configurations are provided inside the AiDisabler.json file.
8. **Launch The Game**: Done!

## Finding AI's Name, ClassId, or TypeID, Creating custom configs

The mod uses these three identifiers to determine which AIs to stop from spawning. With the BepinEx console open & PrintOutAi true in the cfg, you will see nearby creatures pop up. For example, Kelvin will show up as TypeId: Robby.

To get TypeId's in-game of actors use `aishowstats on` in the console. (Open the console by pausing the game, pressing the letters c h e a t s t i c k then pressing f1 to open)
Each actor will have a TypeId, anger, fear, and energy variables that appear on top of them.

To capture Robby (his TypeId), you would do the following:
```json
{
    "Children": [
        { 
          "ID": "RobbyExample",
          "TypeId": "Robby",
          "IsKilled": true
        }
    ],
    "ID": "Global",
    "IsDisabled": false,
    "IsKilled": false
}
```
Would match:
```log
[Info   :AiDisabler] ================VailActor.Start===================== (Ai is alive/Near player)
[Info   :AiDisabler] AI.Name: Robby0
[Info   :AiDisabler] AI.id: Robby
[Info   :AiDisabler] AI.Group: 
[Info   :AiDisabler] AI.ClassId: Other
[Info   :AiDisabler] AI.TypeId: Robby
[Info   :AiDisabler] AI.UniqueId: 2186
[Info   :AiDisabler] AI.FamilyId: 0
[Info   :AiDisabler] AI.IsKilled: true + RobbyExample
```

When ever there is a match, the ID of the entry is printed so you can see what is killing/blocking what.
More examples exist inside AiDisabler.json, including an example that kills all mutants that are not bosses.
Note: IsDisabled/IsKilled inside the Global object override all children if true.

## Compatibility

This mod is compatible with the v1.0 (release, feb 2024) of Sons Of The Forest and 6.0.667 of Bepinex IL2CPP.