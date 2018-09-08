# Invector - UNet Multiplayer Addon
This is a addon to add support for UNet multiplayer in the Invector Template package for unity. Simple just download the package and import it into Unity.

This should add a new menu item under the "Invector" tab called "Multiplayer". 

To Sync all parameters, except for triggers, for an animator run:
 - Invector/Multiplayer/Make Player Multiplayer Compatible
    - This will sync all movements and most of the player's animations.
 
To Add a generic network manager:
 - Invector/Multiplayer/Create Network Manager
    - You don't really need to run this since `Invector/Multiplayer/Make Player Multiplayer Compatible` will generate one for you. However, this is here to just setup a basic networkmanager if you don't want to do any of the other options.
 
To Sync all triggers run:
 - Invector/Multiplayer/Add Multiplayer To Invector Scripts
    - Running `Invector/Multiplayer/Add Multiplayer To Invector Scripts` will actually modify invector code. This is the only way to currently accuratly sync all animation triggers across the network. There isn't an automated method to undo these additions. If you want to undo these changes look at the `InvectorScriptChanges.txt` file. This will list all additions that will be added to these scripts.


## Conver To Work With Melee Combat Only Invector Template
If you don't have the shooter template that's fine just follow these steps to convert this package to work with the melee combat template only. 

If you see:
```
The type or namespace name `vShooter' does not exist in the namespace `Invector'. Are you missing an assembly reference?
```
When you first import this `unitypackage` you now know you need to perform these conversion steps.

Either double click the error message or manually open `UnetMultiplayerAddon/SetupLocalPlayer.cs`. Now comment out the following lines:
```
using Invector.vShooter;
...
...
if (GetComponent<vShooterMeleeInput>()) GetComponent<vShooterMeleeInput>().enabled = true;
if (GetComponent<vShooterManager>()) GetComponent<vShooterManager>().enabled = true;
if (GetComponent<vAmmoManager>()) GetComponent<vAmmoManager>().enabled = true;
...
...
```
