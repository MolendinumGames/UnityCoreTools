using System;
using UnityEngine.SceneManagement;

namespace CoreTools.Console
{
    public class LoadSceneCommand : ConsoleCommand
    {
        readonly string wrongInputMsg = "This command takes only one input.";
        readonly string notFoundMsg = "No scene with that index or name found";
        string wrongMessage = "";

        public override string Command => "loadscene";

        public override string[] WrongInputMessage => new string[] { wrongMessage };

        public override string[] SuccessMessage => null;

        public override bool Process(string[] args)
        {
            if (args.Length != 1)
            {
                wrongMessage = wrongInputMsg;
                return false;
            }
            else if (Int32.TryParse(args[0], out int index) 
                     && SceneManager.GetSceneByBuildIndex(index) != null)
            {
                SceneManager.LoadScene(index);
                return true;
            }
            else if (SceneManager.GetSceneByName(args[0]) != null)
            {
                SceneManager.LoadScene(args[0]);
                return true;
            }
            wrongMessage = notFoundMsg;
            return false;
        }
    }
}