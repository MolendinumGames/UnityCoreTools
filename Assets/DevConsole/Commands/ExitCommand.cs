namespace CoreTools.Console
{
    public class ExitCommand : ConsoleCommand
    {
        public override string Command => "exit";

        public override string[] WrongInputMessage => new string[] { "This command doesn't take any inputs" };

        public override string[] SuccessMessage => null;

        public override bool Process(string[] args)
        {
            if (args.Length > 0) return false;
            DevConsoleController.RaiseExitConsole();
            return true;
        }
    }
}
