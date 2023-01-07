
<p style="text-align:center; font-weight:bold; font-size:40px">Delevoper Console</p>
<p>An easy to use ingame console to help managing the flow of testing gameplay featues and debugging.
<br>

## Features & Usage
- **Simply** Add the console to a scene by selecting the menu item
- Write your **own customized** commands
- Try or get started with a few already **included commands**
- Select **any button** to toggle the console
- **Log or raise** game states

![DevConsole Header Image](/DevConsoleScreenshot_Wide.png)


## Table of contents
- <a href="#Installation">Installation</a>
- <a href="#Getting-Started">Getting Started</a>
- <a href="#Adding-Custom-Commands">Adding Custom Commands</a>
- <a href="#Changing-The-Key-Binding">Changing the key binding</a>
- <a href="#Blocking-Input-while-Open">Blocking Input while Open</a>
- <a href="#Legal">Legal</a>


## Installation

You can find the newest _.unitypackage_ file on [my itch.io account](https://molendinumgames.itch.io/) or in [this repository](https://github.com/MolendinumGames/UnityCoreTools/tree/main/Assets/DevConsole)

**Note:** This package requires TextMeshPro, which is included in any modern standard version of Unity.

I recommend using an engine version of **Unity2021.3LTS or higher**.

## Getting Started
To use the dev console in play mode or in a build it must be present in the scene.
You add the developer console to a scene by selecting the tools tab at the top of
your editor and select "Tools/Add Developer Console".<p>
There is a small list of simple commands to get started. Try opening the console in play mode by pressing _tab_ and then typing _/help_ followed by enter to see a list of all commands.

## Adding Custom Commands
Add new C# file to _`Assets/DevConsole/Commands`_ and replace the contents with the blueprint for a console command.<br>
The add your newly created command to the the `GetCommands()` Function inside _`Assets/DevConsole/Core/DevConsoleController`_ bewlow the comment informing you to add custom commands here.<br>
**Note:** Without this last step your command will not work!

<hr>

```csharp
namespace CoreTools.Console.Commands
{
    public class MyNewCommand : ConsoleCommand
    {
        // This will the command prompt. You will not have to add '/' to the string.
        public override string Command => "exit";
        
        // To be printed in the console in case the arguments of the command are wrong
        // Leave to string.Empty to skip.
        public override string WrongInputMessage => "This command doesn't take any inputs";

        // The output in case the command was successfully executed.
        // Leave to string.empty to skip.
        public override string SuccessMessage => string.Empty;

        // The logic of the command. the args array is the inputs sperated by space
        // not including the command itself.
        public override bool Process(string[] args)
        {
            // Example: The command doesn't take any argument
            if (args.Length > 0)
                return false;
            
            // Enter to code to be executed here:
            
            return true;
        }
    }
}
```

<hr>

You can create a backdrop field and use logic within the _Process(...)_ function to alter the output based on the mistake in the input.<br>
<b>For example:</b>

<hr>

```csharp
[...]
private string wrongInputMessage = String.Empty
public override string WrongInputMessage => wrongInputMessage;
private const string tooManyArguments = "The command takes less arguments."
privata const string notEnoughArguments = "I need more arguments."
[...]


 public override bool Process(string[] args)
 {
    if (args.Length > 4)
    {
        wrongInputMessage = tooManyArguments;
        return false;
    }
    else if (args.Length < 4)
    {
        wrongInputMessage = notEnoughArguments;
        return false;
    }
    else
    {
        // This is just right! Let's execute the command:
        [...]
    }
 }
```

<hr>

<b>I recommend taking a look at the initially provided command files to help getting started!</b>

## Changing the key binding
**Legacy Input System:**<br>
Change in the prefabs inspector under
_`Assets/DevConsole/Resources/ConsolePrefabs/DeveloperConsole`_ or in the inspector of the GameObject in the Scene.

**New Input System:**<br>
Open Assets/DevConsole/Core/DevConsoleController.
At the top of the class body you will see a property that retuns a KeyControl. Change the getter after the nullcheck.<br>
`e.g. Keyboard.current.aKey`

## Blocking Input while Open
The DevConsoleController has a static event OnConsoleOpen that you can subscribe to
to add functionality like blocking Input while using the console or freezing time.
You can reverse these by using the OnConsoleClose event.

## Legal
Copyright (c) 2022 - Christoph Römer. All rights reserved. 

This source code is licensed under the Apache-2.0-style license found
in the LICENSE file in the root directory of this repository. 
You may not use these files except in compliance with the License.

For questions, feedback and suggestions please conact me under:
coretools@molendinumgames.com
