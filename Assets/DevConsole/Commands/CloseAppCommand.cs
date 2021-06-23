namespace CoreTools.Console
{
    public class CloseAppCommand : ConsoleCommand
    {
        public override string Command => "closeapp";

        public override string[] WrongInputMessage => new string[] { "This command doesn't take any inputs" };

        public override string[] SuccessMessage => null;

        public override bool Process(string[] args)
        {
            if (args.Length > 0) return false;
            UnityEngine.Application.Quit();
            return true;
        }
    }
}
