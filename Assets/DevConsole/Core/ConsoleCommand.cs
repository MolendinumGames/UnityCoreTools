/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using UnityEngine;

namespace CoreTools.Console
{
    public abstract class ConsoleCommand : IConsoleCommand
    {
        public abstract string Command { get; }

        public abstract bool Process(string[] args);

        public abstract string WrongInputMessage { get; }
        public abstract string SuccessMessage { get; }
    }
}