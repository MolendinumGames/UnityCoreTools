Dialogue Editor

Basic usage:
- Right click in Project folder
- Create/Dialogue
- Edit the dialogue
- plug it into the desired variable in a GameObject *
- make sure the right event is used *
- make sure the UI is listenting to the event **
- make sure the DialgouUIController is set up **

Editor usage:
- double click on dialogue asset to open dialogue editor window
- right click inside the window to get the create node menu
- Undo/Redo supported!

Demo scene usage:
- create a new dialogue or use the example one
- plug it into the correct field of the dialogue pusher in the hierarchy
- use own assets for UI/icons if you like


* See Assets/CoreTools/Dialogue/DialoguePushText.cs for example
** See demo scene in hierarchy -> DialogueContoller