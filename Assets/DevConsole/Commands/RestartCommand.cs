/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using UnityEngine.SceneManagement;

namespace CoreTools.Console.Commands
{
    public class RestartCommand : ConsoleCommand
    {
        public override string Command => "restart";

        public override string WrongInputMessage => "This command doesn't take any inputs";

        public override string SuccessMessage => "Game restarted.";

        public override bool Process(string[] args)
        {
            if (args.Length > 0)
                return false;

            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
            return true;
        }
    }
}
