using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreTools.Console
{
    public class DevConsoleController : MonoBehaviour
    {
        [SerializeField] GameObject consoleObject;
        [SerializeField] KeyCode consoleKey = KeyCode.Caret;

        public static Action ExitConsole;

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
            if (Input.GetKeyDown(consoleKey))
                SwapConsoleState();
        }
        private void OnDisable()
        {
            ExitConsole -= CloseConsole;
        }

        void SwapConsoleState() => consoleObject.SetActive(!consoleObject.activeInHierarchy);
        void OpenConsole() => consoleObject.SetActive(true);
        void CloseConsole() => consoleObject.SetActive(false);

    }
}
