using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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

        static Action ExitConsole;
        public static void RaiseExitConsole() => ExitConsole?.Invoke();

        List<IConsoleCommand> Commands
        {
            get
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
            consoleUI.Setup(Commands);
        }

        void ReadKeyInput()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(legacyInputKey))
                SwapConsoleState();
#else
#if ENABLE_INPUT_SYSTEM
            if (KeyBinding != null && KeyBinding.wasPressedThisFrame)
                SwapConsoleState();
#endif
#endif
        }

        void SwapConsoleState() => consoleObject.SetActive(!consoleObject.activeInHierarchy);

        void OpenConsole() => consoleObject.SetActive(true);

        void CloseConsole() => consoleObject.SetActive(false);
    }
}
