using System;
using System.Linq;
using System.Collections.Generic;

namespace CoreTools.Console
{
    public class DeveloperConsole
    {
        readonly string prefix;
        readonly IEnumerable<IConsoleCommand> commands;
        public DeveloperConsole(string prefix, IEnumerable<IConsoleCommand> commands)
        {
            this.prefix = prefix;
            this.commands = commands;
        }

        public Action<string[]> PushMessage;
        public IEnumerable<IConsoleCommand> GetAllCommands() => commands;

        public void ProcessInput(string userInput)
        {
            PrintUserInput(userInput);

            var inputs = userInput.Split(' ');
            string commandInput = inputs[0];
            string[] args = inputs.Skip(1).ToArray();

            if (InputMatchesCommandPattern(commandInput))
                TryExecuteUserCommand(commandInput, args);
            else
                PrintError(new string[] { "Not a valid command!" });
        }

        bool InputMatchesCommandPattern(string userInput) => !string.IsNullOrWhiteSpace(userInput) && userInput.StartsWith("/");

        #region User Command Processing
        void TryExecuteUserCommand(string userCommandInput, string[] args)
        {
            userCommandInput = userCommandInput.Trim('/');
            IConsoleCommand targetCommand = FindMatchingCommand(userCommandInput);

            if (targetCommand != null)
                CallCommand(targetCommand, args);
            else
                PrintError(new string[] { "Command not found!" });
        }
        IConsoleCommand FindMatchingCommand(string userCommandInput)
        {
            foreach (IConsoleCommand consoleCommand in GetAllCommands())
            {
                if (userCommandInput.Equals(consoleCommand.Command, System.StringComparison.OrdinalIgnoreCase))
                    return consoleCommand;
            }
            return null;
        }
        void CallCommand(IConsoleCommand command, string[] args)
        {
            if (!command.Process(args))
            {
                if (command.WrongInputMessage != null)
                    PrintError(command.WrongInputMessage);
                else
                    PrintError(new string[] { "Input arguments don't match command!" });
            }
            else if (command.SuccessMessage != null) // Input succesfully processed
            {
                Print(command.SuccessMessage);
            }
        }
        #endregion

        #region Print Functions
        void Print(string[] messages)
        {
            messages = AddPrefixes(messages);
            PushMessage?.Invoke(messages);
        }
        void PrintError(string[] messages)
        {
            string errorMsg = "<color=\"red\">ERROR: </color>";
            messages[0] = errorMsg + messages[0];
            messages = AddPrefixes(messages);
            PushMessage?.Invoke(messages);
        }
        void PrintUserInput(string userInput) =>
            Print(new string[] { "<color=\"yellow\">" + userInput + "</color>" });

        string[] AddPrefixes(string[] messages)
        {
            string[] newMessages = new string[messages.Length];
            for (int i = 0; i < messages.Length; i++)
                newMessages[i] = prefix + messages[i];
            return newMessages;
        }
        #endregion
    }
}