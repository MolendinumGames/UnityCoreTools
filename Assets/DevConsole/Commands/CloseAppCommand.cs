namespace CoreTools.Console.Commands
{
    public class CloseAppCommand : ConsoleCommand
    {
        public override string Command => "closeapp";

        public override string WrongInputMessage => "This command doesn't take any inputs";

        public override string SuccessMessage => "Closed App.";

        public override bool Process(string[] args)
        {
            if (args.Length > 0)
                return false;
            UnityEngine.Application.Quit();
            return true;
        }
    }
}
