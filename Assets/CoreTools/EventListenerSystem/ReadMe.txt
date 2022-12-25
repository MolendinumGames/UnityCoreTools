Copyright (c) 2021 - Christoph Römer. All rights reserved. 

For support, feedback and suggestions please conact me under:
contactsundiray@gmail.com

Check out my other content:
https://sundiray.itch.io/

You can find the source code for my full Unity Core Tolls project on github:
https://github.com/Sundiray/UnityCoreTools

____________________________________________________________________

Version 1.0
Indcluding channels for events of type:
void, string, int, bool, float, Vector2, Vector3, Vector2Int, Vector3Int

Note: A Dialogue event channel for my Dialogue system is available seperately
____________________________________________________________________

>>> Why use a scirptable object based event system:
	"Game architecture with ScriptableObjects | Open Projects Devlog" by Unity
	https://www.youtube.com/watch?v=WLDgtRNK2VE
	"Unity Tutorial - Item / Story Dialogues | Scriptable Object Events" by Infinity PBR
	https://www.youtube.com/watch?v=HVls6_srbNc

>>> Creating channels:
	Rightclick where you want to create the new channel in your assets directory.
	Then select the correct type under Create/Channel/...

>>> Raising an event channel:
	Include "using CoreTools" at the top of your C# file.
	Create a serialized field (public or "[SerialitzeField]") of the channeltype you need.
	Drag your previously created channel into the corresponding field in the inspector.
	Where you want to raise the event simply call .Raise() on the channel object with the
	correct value type for the channel passed in or leave it empty for the void channel.

>>> Responding to events:
	In the inspector of the gameobject that shall listen, add the listener of the correct type.
	For excample "StringChannelListener". Now drag the channel you want it to listen to into the
	corresponding field in the inspector of the just added component.
	In the OnEventRaised list add another entry and drag in the component on which a method shall
	be called then select it in the dropdown right next to it.

>>> Keep in mind!
	For methods that accept a value you will have two options in the inspecor when selecting a method
	to be called on a raised event.
	One allows you to edit the value which the method processes directly here in the inspector and ignore what was beind 
	passed in when raising the event.
	(!) And the other option is using dynamics (at the top of the dropdown) to use the value that the event passed in.
	Not selecting the dynamic type is a common overlook.