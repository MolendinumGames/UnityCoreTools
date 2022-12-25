Copyright (c) 2021 - Christoph Römer. All rights reserved. 

For support, feedback and suggestions please conact me under:
contactsundiray@gmail.com

Check out my other content:
https://sundiray.itch.io/

_______________________________________________________________

Requires Unity 2021 LTS or newer. May work on older version but no guarantee.
(!) This asset requires TextMeshPro plugin by Unity!

Version 0.1b
	Initial beta test

_______________________________________________________________

Initial Keybinging: Tab Key (Keyboard only)

_______________________________________________________________

>>> Getting started:
	To use the dev console in play mode or in a build it must be present in the scene.
	You add the developer console to a scene by selecting the tools tab at the top of
	your editor and select "Tools/Add Developer Console".
	There is a small list of simple commands to get started.

>>> Adding Custom Commands:
	Add new C# filen and change namespace to CoreTools.Console.Commands
	Remove the Start and Update function
	Inherit from ConsoleCommand instead of MonoBehaviour
	Let your IDE implement the abstract class
	string 'Command'
		is the command word you will type in the console
		(note that you will not have to include a '/')
	string 'WrongInputMessage'
		will be displayed if the user given input arguments
		don't match requirements. Leave empty at will. You can change the
		message during the processing at runtime if you need to.
	string 'SuccessMessage'
		prints on successful processing of user input. Can be
		left empty or changed during runtime based on input.
	bool 'Process' args[]
		will return you all the user input after (!) the matching command.
		This array will not include the inital command as first entry and may be empty.
		Arguments are split by whitespace.
		Use this function to execute your desired command functionality after
		validating the args.
	Add your newly created command to the the GetCommands() Function inside
	Assets/DevConsole/Core/DevConsoleController at line 51. It is also marked with a comment.
	(!) Without this step your command will not work!

	I recommend taking a look at the initially provided command files to help getting started!

>>> Changing the key binding:
	Legacy Input:
		Change in the prefabs inspector under
		Assets/DevConsole/Resources/DevConsole
		or in the inspector of the GameObject in the Scene.
	New InputSystem:
		Open Assets/DevConsole/Core/DevConsoleController.
		At the top of the class body you will see a property that
		retuns a KeyControl. Change the getter after the nullcheck.
		e.g. Keyboard.current.aKey

