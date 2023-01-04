/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
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