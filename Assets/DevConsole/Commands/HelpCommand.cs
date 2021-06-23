namespace CoreTools.Console
{
    public class HelpCommand : ConsoleCommand
    {
        public override string Command => "help";

        public override string[] WrongInputMessage => new string[] { "This command doesn't take any inputs" };

        public override string[] SuccessMessage => null;

        public override bool Process(string[] args)
        {
            if (args.Length > 0) return false;
            DeveloperConsoleUI.ConsoleHelp?.Invoke();
            return true;
        }
    }
}
