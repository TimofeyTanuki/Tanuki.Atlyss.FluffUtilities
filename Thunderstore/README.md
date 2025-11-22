# Tanuki.Atlyss.FluffUtilities
Multifunctional plugin for Atlyss. Change character appearance sliders, auto-pickup, free camera, no clip, hotkeys, a bunch of auxiliary commands, and more.<br>
## Features
- Change the range of character appearance sliders.
- Allow appearance parameters beyond the limits.
- Change character appearance with hotkeys.
  - `Keypad7`/`Keypad9` - Head width.
  - `Keypad4`/`Keypad6` - Breast size.
  - `Keypad2`/`Keypad5` - Belly size.
  - `Keypad1`/`Keypad3` - Bottom size.
  - Other parameters are also available in the configuration: torso size, voice pitch, muzzle length, body width and height.
- Free camera.
  - Toggle with a command or hotkey (`End` by default).
  - Speed can be adjusted using the mouse wheel.
  - Changeable movement controls.
  - Switchable character controls lock.
  - Switchable look smoothing mode.
- No clip.
  - Toggle with a command or hotkey (`Delete` by default).
  - Changeable movement controls.
- Auto-pickup of items appearing within range.
- See other players using this plugin in chat when they join a lobby or with the `/list-players` command.
- Settings can be changed in-game on the mod tab.
- ...
## Translations
The following languages are available for this plugin:
- `default` - English (default).
- `russian` - Russian.

The language can be changed in the Tanuki.Atlyss.Bootloader configuration:<br>
`BepInEx\config\9c00d52e-10b8-413f-9ee4-bfde81762442.cfg`<br>
The changes will take effect after restarting the game or reloading Tanuki.Atlyss using the `/reload` command.
## Commands
### Commands for the Tanuki.Atlyss framework
- `/reload Tanuki.Atlyss.FluffUtilities` - Reload this plugin.
- `/help Tanuki.Atlyss.FluffUtilities` - Display a list of commands for this plugin.

### Plugin commands
- `/freecamera` - Free camera mode.
- `/steamprofile <nickname>` - Open the player's Steam profile.
- `/noskillcooldown` - Toggle the skill cooldown timer.
- `/autopickup [radius]` - Automatic item pickup. Do not specify parameters to disable.
- `/currency <delta>` - Add or remove currency.
- `/disenchant` - Remove the enchantment from the item in the first (top left) slot on the equipment tab.
- `/enchant [damage type] [modifier ID]` - Enchant the item in the first (top left) slot on the equipment tab. Example: `/enchant 99 99` (Use invalid parameters to get a hint).
- `/heal <delta>` - Add or remove health.
- `/immortality` - Immortality mode.
- `/item <name> [quantity]` - Give an item. Example: `/item "Deadwood Axe" 1`. If there is no exact match, the first item with a similar name will be given.
- `/list-items [case-sensitive search]` - Display a list of items.
- `/list-players` - Display a list of players.
- `/infinitejumps` - Infinite jumps.
- `/movespeed [speed]` - Change the movement speed. Do not specify parameters to reset.
- `/teleporttoplayer <nickname>` - Teleport to a player.
- `/experience <delta>` - Add or remove experience.
- `/noskilltimer` - Toggle the skill timer.
- `/noclip` - No clip mode.
- ...
Many commands have shortcuts and aliases. View them with the `/help` command.
### Configuration
Plugin configuration<br>
> `BepInEx/config/cc8615a7-47a4-4321-be79-11e36887b64a.cfg`

Plugin command configuration<br>
> `BepInEx/config/Tanuki.Atlyss.FluffUtilities/{Current language}.command.json`

Plugin translation file<br>
> `BepInEx/config/Tanuki.Atlyss.FluffUtilities/{Current language}.translation.properties`
## Getting started
### Thunderstore
This mod on [Thunderstore](https://thunderstore.io/c/atlyss/p/Tanuki/Tanuki_Atlyss_FluffUtilities/).<br>
It's recommended to use it together with the [EasySettings](https://thunderstore.io/c/atlyss/p/Nessie/EasySettings/) mod for easier configuration directly in the game.
### Manual installation
1. Install [BepInEx](https://docs.bepinex.dev/articles/user_guide/installation/index.html). It's recommended to use the [preconfigured package](https://thunderstore.io/c/atlyss/p/BepInEx/BepInExPack/).
2. Install the [Tanuki.Atlyss](https://github.com/TimofeyTanuki/Tanuki.Atlyss) framework.
3. Install the [Tanuki.Atlyss.FluffUtilities](../../releases) files.
## Anything else?
[Contacts](https://tanu.su/)

![AttractiveFurryWoman](https://github.com/user-attachments/assets/09263e00-2b1c-41aa-842c-68f2bead85e9)