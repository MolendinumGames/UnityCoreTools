/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

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
            // Wont print empty messages but accepts whitespace only
            if (string.IsNullOrEmpty(message))
                return;

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
            LogText($"To close type \"<color=\"yellow\">/exit</color>\" or press console key.");
            LogText($"Type \"<color=\"yellow\">/help</color>\" for list of commands.");
        }

        void PrintConsoleHelp()
        {
            var builder = new StringBuilder();
            int counter = 0;
            foreach (var comm in console.Commands)
            {
                counter++;
                string commandWord = '/' + comm.Command;
                if (counter % 3 == 0)
                {
                    builder.Append(commandWord + "<br>");
                }
                else
                {
                    int fillingSpaceAmount = 20 - commandWord.Length;
                    builder.Append(commandWord + GetFillingSpaces(fillingSpaceAmount));
                }
            }
            LogText(builder.ToString().TrimEnd(' '));
        }

        string GetFillingSpaces(int count)
        {
            StringBuilder builder = new();
            for (int i = 0; i < count; i++)
            {
                builder.Append(' ');
            }
            return builder.ToString();
        }
            

        IEnumerator SelectInputField()
        {
            // Wait 1 frame because the input area is not selectable the frame it is created
            yield return new WaitForEndOfFrame();
            inputArea.ActivateInputField();
            inputArea.Select();
        }

        string GetFormattedUserInput(string userInput) =>
            $"{console.Prefix} <color=\"yellow\">{userInput}</color>";
    }
}
