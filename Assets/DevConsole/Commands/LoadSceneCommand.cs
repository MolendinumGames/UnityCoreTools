/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using System;
using UnityEngine.SceneManagement;

namespace CoreTools.Console.Commands
{
    public class LoadSceneCommand : ConsoleCommand
    {
        const string wrongAmountMsg = "This command takes only one input.";
        const string notFoundMsg = "No scene with that index or name found";
        string currentWrongMessage = "";

        string currentSuccessMessage = "";

        public override string Command => "loadscene";

        public override string WrongInputMessage => currentWrongMessage;

        public override string SuccessMessage => currentSuccessMessage;

        public override bool Process(string[] args)
        {
            if (args.Length != 1)
            {
                currentWrongMessage = wrongAmountMsg;
                return false;
            }

            string sceneArg = args[0];

            if (TryLoadByIndex(sceneArg))
            {
                currentSuccessMessage = BuildSuccessMessage(sceneArg);
                return true;
            }
            else if (TryLoadByName(sceneArg))
            {
                currentSuccessMessage = BuildSuccessMessage(sceneArg);
                return true;
            }

            currentWrongMessage = notFoundMsg;
            return false;
        }

        bool TryLoadByIndex(string sceneArg)
        {
            if (Int32.TryParse(sceneArg, out int index)
                && SceneManager.GetSceneByBuildIndex(index) != null)
            {
                SceneManager.LoadScene(index);
                return true;
            }
            return false;
        }

        bool TryLoadByName(string sceneArg)
        {
            if (SceneManager.GetSceneByName(sceneArg) != null)
            {
                SceneManager.LoadScene(sceneArg);
                return true;
            }
            return false;
        }

        string BuildSuccessMessage(string sceneName) =>
            $"Scene {sceneName} loaded.";
    }
}