/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

namespace CoreTools.Console.Commands
{
    public class HelpCommand : ConsoleCommand
    {
        public override string Command => "help";

        public override string WrongInputMessage => "This command doesn't take any inputs";

        public override string SuccessMessage => string.Empty;

        public override bool Process(string[] args)
        {
            if (args.Length > 0)
                return false;
            DeveloperConsoleUI.ConsoleHelp?.Invoke();
            return true;
        }
    }
}
