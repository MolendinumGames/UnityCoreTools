using UnityEngine.SceneManagement;
namespace CoreTools.Console
{
    public class RestartCommand : ConsoleCommand
    {
        public override string Command => "restart";

        public override string[] WrongInputMessage => new string[] { "This command doesn't take any inputs" };

        public override string[] SuccessMessage => new string[] { "Game restarted!" };

        public override bool Process(string[] args)
        {
            if (args.Length > 0) return false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return true;
        }
    }
}
