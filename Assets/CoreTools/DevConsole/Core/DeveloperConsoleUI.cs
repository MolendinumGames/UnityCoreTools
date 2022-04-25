using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CoreTools.Console
{
    public class DeveloperConsoleUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI logArea;
        [SerializeField] TMP_InputField inputArea;
        [SerializeField] string prefix = "";

        public static Action ClearConsole;
        public static Action ConsoleHelp;

        DeveloperConsole console;

        ConsoleScroller scrollController;

        private void Awake()
        {
            console = new DeveloperConsole(
            prefix,
            new List<IConsoleCommand>()
            {
                // Add commands here
                new RestartCommand(),
                new ClearCommand(),
                new HelpCommand(),
                new ExitCommand(),
                new CloseAppCommand(),
                new LoadSceneCommand()
            });
            scrollController = GetComponentInChildren<ConsoleScroller>();
        }
        private void OnEnable()
        {
            ClearLog();
            ClearInput();
            SubEvents();
            SetUpInputField();
        }
        private void OnDisable()
        {
            UnsubEvents();
        }

        void SubEvents()
        {
            inputArea.onSubmit.AddListener(ProcessInput);
            console.PushMessage += LogText;
            ClearConsole += ClearLog;
            ConsoleHelp += PrintConsoleHelp;
        }
        private void UnsubEvents()
        {
            console.PushMessage -= LogText;
            inputArea.onSubmit.RemoveAllListeners();
            ClearConsole -= ClearLog;
            ConsoleHelp -= PrintConsoleHelp;
        }

        void SetUpInputField()
        {
            inputArea.caretWidth = 10;
            StartCoroutine(SelectInputField());
        }

        void ProcessInput(string userInput)
        {
            console.ProcessInput(userInput);

            ResetInputField();
        }

        void LogText(IEnumerable<string> messages)
        {
            foreach (string line in messages)
                logArea.text += line + "<br>";
            scrollController.MoveDown();
        }

        void ClearLog()
        {
            logArea.text = "";
            PrintExitCommand();
            PrintHelpInfo();
        }
        void ResetInputField()
        {
            ClearInput();
            StartCoroutine(SelectInputField());
        }
        void ClearInput() => inputArea.text = "";
        void PrintExitCommand() => LogText(new string[] { $"{prefix}Close with F10 or \"<color=\"yellow\">/exit</color>\"." });
        void PrintHelpInfo() => LogText(new string[] { $"{prefix}Type \"<color=\"yellow\">/help</color>\" for commands list" });
        void PrintConsoleHelp()
        {
            var builder = new StringBuilder();
            foreach (var comm in console.GetAllCommands())
                builder.Append('/'+comm.Command+' ');
            LogText(new string[] { prefix + builder.ToString().TrimEnd(' ') });
        }
        IEnumerator SelectInputField()
        {
            yield return new WaitForEndOfFrame();
            inputArea.ActivateInputField();
            inputArea.Select();
        }
    }
}
