Copyright (c) 2021 - Christoph Römer. All rights reserved. 

For support, feedback and suggestions please conact me under:
contactsundiray@gmail.com

Check out my other content:
https://sundiray.itch.io/

____________________________________________________________________

Version 1.0
Indcluding channels for events of type:
void, string, int, bool, float, Vector2, Vector3, Vector2Int, Vector3Int

Note: A Dialogue event channel for my Dialogue system is available seperately
____________________________________________________________________

>>> Creating channels:
	Rightclick where you want to create the new channel in your assets directory.
	Then select the correct type under Create/Channel/...

>>> Raising an event channel:
	Include "using CoreTools" at the top of your C# file.
	Create a serialized field (public or "[SerialitzeField]") or
	of the channeltype you need.
	Drag your previously created channel into the corresponding field.
	In the line where you need to raise the event simply call .Raise() with the
	correct value type for the channel passed in or leave it empty for the void channel.

>>> 