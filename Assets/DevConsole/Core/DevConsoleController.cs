/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using CoreTools.Console.Commands;

namespace CoreTools.Console
{
    public class DevConsoleController : MonoBehaviour
    {
        [SerializeField]
        GameObject consoleObject;

#if ENABLE_LEGACY_INPUT_MANAGER
        [SerializeField]
        KeyCode legacyKeyBinding = KeyCode.Tab;
#else
#if ENABLE_INPUT_SYSTEM
        UnityEngine.InputSystem.Controls.KeyControl KeyBinding
        {
            get
            {
                // Edit Keybinding when using Unitys new InputSystem here:
                if (Keyboard.current != null)
                    return Keyboard.current.tabKey;
                else
                    return null;
            }

        }
#endif
#endif

        static Action ConsoleOpened;
        public static Action OnConsoleOpened => ConsoleOpened;
        static Action ConsoleClosed;
        public static Action OnConsoleClosed => ConsoleClosed;

        static Action ExitConsole;
        public static void RaiseExitConsole() => ExitConsole?.Invoke();

        List<IConsoleCommand> GetCommands()
        {
            return new List<IConsoleCommand>
            {
                new RestartCommand(),
                new ClearCommand(),
                new HelpCommand(),
                new ExitCommand(),
                new CloseAppCommand(),
                new LoadSceneCommand()
                // Add your new commands here
            };
        }

        private void Awake()
        {
            SetupConsoleComponents();
        }

        private void OnEnable()
        {
            ExitConsole += CloseConsole;
        }

        private void Start()
        {
            CloseConsole();
        }

        private void Update()
        {
            ReadKeyInput();
        }

        private void OnDisable()
        {
            ExitConsole -= CloseConsole;
        }

        private void SetupConsoleComponents()
        {
            var consoleUI = consoleObject.GetComponentInChildren<DeveloperConsoleUI>();
            consoleUI.Setup(GetCommands());
        }

        void ReadKeyInput()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(legacyKeyBinding))
                SwapConsoleState();
#else
#if ENABLE_INPUT_SYSTEM
            if (KeyBinding != null && KeyBinding.wasPressedThisFrame)
                SwapConsoleState();
#endif
#endif
        }

        void SwapConsoleState()
        {
            if (consoleObject == null)
                throw new NullReferenceException("No Console Object set in the DevConsoleController!");

            bool isOpen = consoleObject.activeInHierarchy;
            if (isOpen)
                CloseConsole();
            else
                OpenConsole();
        }

        void OpenConsole()
        {
            consoleObject.SetActive(true);
            ConsoleOpened?.Invoke();
        }

        void CloseConsole()
        {
            consoleObject.SetActive(false);
            ConsoleClosed?.Invoke();
        }
    }
}
