using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            var inputs = userInput.Split(' ');
            string commandInput = inputs[0];

            if (!string.IsNullOrWhiteSpace(commandInput) && commandInput.StartsWith("/"))
            {
                // log the actual user input
                Print(new string[] { "<color=\"yellow\">" + userInput + "</color>" });

                commandInput = commandInput.Substring(1);
                string[] args = inputs.Skip(1).ToArray();
                if (!ProcessCommand(commandInput, args))
                {
                    PrintError(new string[] { "Command not found!" });
                }
            }
            else
            {
                PrintError(new string[] { "Illegal Input!" });
            }
        }
        bool ProcessCommand(string commandInput, string[] args)
        {
            bool found = false;
            foreach (var c in commands)
            {
                if (commandInput.Equals(c.Command, System.StringComparison.OrdinalIgnoreCase))
                {
                    if (!c.Process(args))
                    {
                        if(c.WrongInputMessage != null)
                            PrintError(c.WrongInputMessage);
                    }
                    else if (c.SuccessMessage != null)
                        Print(c.SuccessMessage);

                    found = true;
                    break;
                }
            }
            return found;
        }
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
        string[] AddPrefixes(string[] message)
        {
            string[] result = new string[message.Length];
            for (int i = 0; i < message.Length; i++)
                result[i] = prefix + message[i];
            return result;
        }
    }
}