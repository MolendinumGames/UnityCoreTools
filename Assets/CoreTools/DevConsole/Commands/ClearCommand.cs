namespace CoreTools.Console
{
    public class ClearCommand : ConsoleCommand
    {
        public override string Command => "clear";

        public override string[] WrongInputMessage => new string[] { "This command doesn't take any inputs" };

        public override string[] SuccessMessage => null;

        public override bool Process(string[] args)
        {
            if (args.Length > 0) return false;
            DeveloperConsoleUI.ClearConsole?.Invoke();
            return true;
        }
    }
}
