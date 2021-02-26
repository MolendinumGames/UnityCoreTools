using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private static readonly string windowTitle = "Dialogue Window";

        [MenuItem("Tools/DialogueEditor")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueEditor>(windowTitle);
            window.Show();
        }
    }
}