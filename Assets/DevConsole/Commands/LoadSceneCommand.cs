using System;
using UnityEngine.SceneManagement;

namespace CoreTools.Console
{
    public class LoadSceneCommand : ConsoleCommand
    {
        readonly string wrongAmountMsg = "This command takes only one input.";
        readonly string notFoundMsg = "No scene with that index or name found";
        string wrongInputMsg = "";

        public override string Command => "loadscene";

        public override string[] WrongInputMessage => new string[] { wrongInputMsg };

        public override string[] SuccessMessage => null;

        public override bool Process(string[] args)
        {
            if (args.Length != 1)
            {
                wrongInputMsg = wrongAmountMsg;
                return false;
            }

            string sceneArg = args[0];

            if (TryLoadByIndex(sceneArg))
                return true;
            else if (TryLoadByName(sceneArg))
                return true;

            wrongInputMsg = notFoundMsg;
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
    }
}