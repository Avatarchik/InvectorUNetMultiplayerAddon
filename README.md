# InvectorUNetMultiplayerAddon
This is a addon to add support for UNet multiplayer in the Invector Template package for unity.

Simple just download the package and import it into Unity.

This should add a new menu item under the "Invector" tab called "Multiplayer". 

So to add multiplayer support to your character do the following:
1. Select Invector/Multiplayer/Make Player Multiplayer Compatible
2. In the pop-up window drag your player **prefab** into the box
3. Select "Add Multiplayer Support"
4. Done!

This will generate a "NetworkManager" gameobject and add your prefab to it. This will also add unity's basic HUD to it as well. This is meant to be a great starting point for you to understand how to integrate multiplayer components with the invector package.
