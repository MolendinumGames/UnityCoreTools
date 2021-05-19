namespace CoreTools.Console
{
    public interface IConsoleCommand
    {
        string Command { get; }
        bool Process(string[] args);
        string[] WrongInputMessage { get; }
        string[] SuccessMessage { get; }
    }
}