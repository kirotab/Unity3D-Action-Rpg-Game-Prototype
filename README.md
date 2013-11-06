Unity3D-Action-Rpg-Game-Prototype
=================================

First try with Unity3d ,multiplayer action game, players log to a server and throw spears at each other till death,
and then again :)

Here are just the script files and the executable, the code is not formatted properly as it was written in the mono
developer tool that comes with unity and in a hurry, the idea was to create a game prototype and play with unity for
3-4 days, and that's why it's so messy and some things are done in more than 1 variation, and then left commented.

Scripts were written in c# with the default mono dev kit that comes with unity, and the network was based on a
network tutorial. But in the current implementation someone has to host a server locally with the exe file "MasterServer"
then start the exe of the game prototype and host a game with the host button in the upper left corner,
the other player should change the ip in the settings panel to the ip of the host and then click refresh button to see the
available games or host game.

When players log they can throw spears at each other, run around the small area, jump, crouch, fly(multiple jumping).
Players can respawn with the respawn button or dissconect at any time. When player's health reaches 0 the colliding body
is removed and the player dies and respawns after some seconds.

controls are
w - forward
s - backwards
a - strafe left
d - strafe right
q - turn left
e - turn right
ctrl - crouch
space - jump
mouse1 - fire
mouse - look around

ctrl + c is combat mode which locks the mouse pointer in the center of the screen otherwise it is unplayable :)

In conclusion, unity is really nice tool for creating prototypes, and not hard to get into, just for 3-4 days you can
have network game without any previous experience, just some basic programming skills and a lot of reading :)
