using System;
using System.Collections;
using System.Collections.Generic;
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

        bool isClosing = false;

        private void Awake()
        {
            scrollController = GetComponentInChildren<ConsoleScroller>();
        }

        private void OnEnable()
        {
            isClosing = false;
            ClearLog();
            ClearInput();
            SubEvents();
            SetUpInputField();
        }

        private void OnDisable()
        {
            isClosing = true;
            UnsubEvents();
        }

        public void Setup(IEnumerable<IConsoleCommand> commands) =>
            console = new DeveloperConsole(prefix, commands);

        void SubEvents()
        {
            inputArea.onSubmit.AddListener(ProcessInput);
            ClearConsole += ClearLog;
            ConsoleHelp += PrintConsoleHelp;
        }

        private void UnsubEvents()
        {
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
            LogText(GetFormattedUserInput(userInput));

            string processedMessage = console.ProcessInput(userInput);
            LogText(processedMessage);

            ResetInputField();
        }

        void LogText(string message)
        {
            logArea.text += message + "<br>";
            scrollController.MoveDown();
        }

        void ClearLog()
        {
            logArea.text = "";
            PrintInitialInfo();
        }

        void ResetInputField()
        {
            ClearInput();

            if (!isClosing)
                StartCoroutine(SelectInputField());
        }

        void ClearInput() =>
            inputArea.text = "";

        void PrintInitialInfo()
        {
            LogText($"{prefix}Close with F10 or \"<color=\"yellow\">/exit</color>\".");
            LogText($"{prefix}Type \"<color=\"yellow\">/help</color>\" for commands list");
        }

        void PrintConsoleHelp()
        {
            var builder = new StringBuilder();
            foreach (var comm in console.Commands)
                builder.Append('/'+comm.Command+' ');
            LogText(prefix + builder.ToString().TrimEnd(' '));
        }

        IEnumerator SelectInputField()
        {
            // Wait 1 frame because the input area is not selectable the frame it is created
            yield return new WaitForEndOfFrame();
            inputArea.ActivateInputField();
            inputArea.Select();
        }

        string GetFormattedUserInput(string userInput) =>
            $"{console.Prefix}  <color=\"yellow\">{userInput}</color>";
    }
}
