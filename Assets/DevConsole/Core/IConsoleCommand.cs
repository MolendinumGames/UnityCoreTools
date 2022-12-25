/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

namespace CoreTools.Console
{
    public interface IConsoleCommand
    {
        string Command { get; }
        bool Process(string[] args);
        string WrongInputMessage { get; }
        string SuccessMessage { get; }
    }
}