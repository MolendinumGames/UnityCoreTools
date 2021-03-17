using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using CoreTools;
using CoreTools.NodeSystem;

namespace CoreTools.QuestSystem.Editor
{
    public class QuestEditorWindow : NodeEditorWindow
    {
        public Quest selectedQuest;
        protected override string NoGraphMessage => "Node Quest Selected!";
        protected override int popupButtonCount => 6;

        private static readonly string windowTitle = "Quest Editor";
        [MenuItem("Tools/QuestEditor")]
        public static void OpenWindow()
        {
            var window = GetWindow<QuestEditorWindow>(windowTitle);
            window.Show();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            if (EditorUtility.InstanceIDToObject(instanceID) is Quest)
            {
                OpenWindow();
                return true;
            }
            return false;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            //questDrawer = new QuestDrawer(this, this);
            //nodeDrawer = questDrawer;
        }
        protected override void OnDrawPopupContent(Vector2 position)
        {
            throw new NotImplementedException();
        }

        protected override bool ProcessSelection()
        {
            if (Selection.activeObject is Quest newQuest)
            {
                selectedQuest = newQuest;
                selectedGraph = newQuest;
                return true;
            }
            return false;
        }
    }
}