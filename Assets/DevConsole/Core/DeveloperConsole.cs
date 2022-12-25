/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using System;
using System.Linq;
using System.Collections.Generic;

namespace CoreTools.Console
{
    public class DeveloperConsole
    {
        public string Prefix { get; }
        public List<IConsoleCommand> Commands { get; }

        const string ErrorPrefix = "<color=\"red\">ERROR: </color>";
        const string NotFoundMessage = "No such command found.";
        const string InvalidCommandMessage = "Not a valid command syntax.";
        const string WrongInputFallbackMessage = "Wrong input args given.";
        const string SuccessFallbackMessage = "Done.";

        public DeveloperConsole(string prefix, IEnumerable<IConsoleCommand> commands)
        {
            this.Prefix = prefix;
            this.Commands = commands.ToList();
        }

        public string ProcessInput(string userInput)
        {
            var inputs = userInput.Split(' ');
            string commandInput = inputs[0];
            string[] args = inputs.Skip(1).ToArray();

            if (InputMatchesCommandPattern(commandInput))
            {
                return TryExecuteUserCommand(commandInput, args);
            }
            else
            {
                return BuildErrorMessage(InvalidCommandMessage);
            }
        }

        bool InputMatchesCommandPattern(string userInput) =>
            !string.IsNullOrWhiteSpace(userInput)
            && userInput.StartsWith("/");

        string TryExecuteUserCommand(string userCommandInput, string[] args)
        {
            userCommandInput = userCommandInput.Remove(0, 1); // Remove the '/'
            IConsoleCommand targetCommand = FindMatchingCommand(userCommandInput);

            if (targetCommand != null)
                return CallCommand(targetCommand, args);
            else
                return BuildErrorMessage(NotFoundMessage);
        }

        IConsoleCommand FindMatchingCommand(string userCommandInput)
        {
            foreach (IConsoleCommand consoleCommand in Commands)
            {
                if (userCommandInput.Equals(consoleCommand.Command, System.StringComparison.OrdinalIgnoreCase))
                    return consoleCommand;
            }
            return null;
        }

        string CallCommand(IConsoleCommand command, string[] args)
        {
            if (command.Process(args))
            {
                // Succesfull command execution
                if (command.SuccessMessage != null)
                    return command.SuccessMessage;
                else
                    return SuccessFallbackMessage;
            }
            else
            {
                // Correct command but wrong input args given
                if (command.WrongInputMessage != null)
                    return BuildErrorMessage(command.WrongInputMessage);
                else
                    return BuildErrorMessage(WrongInputFallbackMessage);
            }
        }

        string BuildErrorMessage(string message) =>
            ErrorPrefix + message;
    }
}