Copyright (c) 2021 - Christoph Römer. All rights reserved. 

For support, feedback and suggestions please conact me under:
contactsundiray@gmail.com

Check out my other content:
https://sundiray.itch.io/

_______________________________________________________________

Version 0.1b
	Initial beta test

_______________________________________________________________

Initial Keybinging: Tab Key (Keyboard only)
Intended for Unity LTS 2020.3 and 2021.1
_______________________________________________________________

>>> Getting started:
	To use the dev console in play mode or in a build it must be present in the scene.
	You add the developer console to a scene by selecting the tools tab at the top of
	your editor and select "Tools/Add Developer Console".
	There is a small list of simple commands to get started.

>>> Adding Custom Commands:
	Add new C# file to Assets/DevConsole/Commands
	Add it to the namespace CoreTools.Console.Commands
	Get rid if the Start and Update function
	Inherit from ConsoleCommand instead
	Let your IDE implement the abstract class
	'Command'
		is the command word you will type in the console
		(note that you will not have to include a '/')
	'WrongInputMessage'
		will be displayed if the user given input arguments
		don't match requirements. Leave empty at will. You can change the
		message during the processing at runtime if you need to.
	'SuccessMessage'
		prints on successful processing of user input. Can be
		left empty or changed during runtime based on input.
	'bool Process args[]'
		will return you all the user input after (!) the matching command.
		This array will not include the inital command as first entry and may be empty.
		Arguments are split by whitespace.
		Use this function to execute your desired command functionality after
		validating the args.

	I recommend taking a look at the initially provided command files to help getting started!

>>> Changing the key binding:
	Legacy Input:
		Change in the prefabs inspector under
		Assets/DevConsole/Resources/ConsolePrefabs/DeveloperConsole
		or in the inspector of the GameObject in the Scene.
	New InputSystem:
		Open Assets/DevConsole/Core/DevConsoleController.
		At the top of the class body you will see a property that
		retuns a KeyControl. Change the getter after the nullcheck.
		e.g. Keyboard.current.aKey

>>> Blocking Input while open:
	The DevConsoleController has a static event OnConsoleOpen that you can subscribe to
	to add functionality like blocking Input while using the console or freezing time.
	You can reverse these by using the OnConsoleClose event.

