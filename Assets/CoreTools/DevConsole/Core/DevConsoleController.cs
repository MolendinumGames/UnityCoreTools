using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreTools.Console
{
    public class DevConsoleController : MonoBehaviour
    {
        [SerializeField] GameObject consoleObject;
        [SerializeField] KeyCode consoleKey = KeyCode.F10;
        // using a Property to avoid console message that field isn't used due to #if
        KeyCode ConsoleKey { get => consoleKey; } 

        static Action ExitConsole;
        public static void RaiseExitConsole() => ExitConsole?.Invoke();

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

        void ReadKeyInput()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(ConsoleKey))
                SwapConsoleState();
#else
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.f10Key.wasPressedThisFrame)
                SwapConsoleState();
#endif
#endif
        }
        void SwapConsoleState() => consoleObject.SetActive(!consoleObject.activeInHierarchy);
        void OpenConsole() => consoleObject.SetActive(true);
        void CloseConsole() => consoleObject.SetActive(false);

    }
}
