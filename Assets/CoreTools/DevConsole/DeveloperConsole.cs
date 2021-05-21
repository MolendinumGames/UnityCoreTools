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

            if (UserInputIsValid(commandInput))
                TryExecuteUserCommand(commandInput, args);
            else
                PrintError(new string[] { "Not a valid command!" });
        }

        bool UserInputIsValid(string userInput) => !string.IsNullOrWhiteSpace(userInput) && userInput.StartsWith("/");

        #region User Command Processing
        void TryExecuteUserCommand(string userCommand, string[] args)
        {
            userCommand = userCommand.Remove('/');
            IConsoleCommand targetCommand = FindMatchingCommand(userCommand);

            if (targetCommand != null)
                CallCommand(targetCommand, args);
            else
                PrintError(new string[] { "Command not found!" });
        }
        IConsoleCommand FindMatchingCommand(string userCommand)
        {
            foreach (IConsoleCommand consoleCommand in commands)
            {
                if (userCommand.Equals(consoleCommand.Command, System.StringComparison.OrdinalIgnoreCase))
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
            }
            else if (command.SuccessMessage != null)
            {
                Print(command.SuccessMessage);
            }
        }
        #endregion

        #region Print Functions
        void Print(string[] message)
        {
            message = AddPrefixes(message);
            PushMessage?.Invoke(message);
        }
        void PrintError(string[] message)
        {
            string msgType = "<color=\"red\">ERROR: </color>";
            message[0] = msgType + message[0];
            message = AddPrefixes(message);
            PushMessage?.Invoke(message);
        }
        void PrintUserInput(string userInput) =>
            Print(new string[] { "<color=\"yellow\">" + userInput + "</color>" });
        string[] AddPrefixes(string[] message)
        {
            string[] result = new string[message.Length];
            for (int i = 0; i < message.Length; i++)
                result[i] = prefix + message[i];
            return result;
        }
        #endregion
    }
}