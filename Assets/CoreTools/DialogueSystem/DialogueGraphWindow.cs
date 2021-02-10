using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
	public class DialogueGraphWindow : EditorWindow
	{
        private DialogueObject dialogueObject;
        private DialogueGraphView graphView;

        private Toolbar toolbar;

		[MenuItem("MyTools/DialogueEditor")]
		public static void OpenGraphView()
        {
			var window = GetWindow<DialogueGraphWindow>();
			window.titleContent = new GUIContent("DialogueEditor");
			window.Show();
        }
        private void OnGUI()
        {
            if (graphView == null && dialogueObject != null)
                graphView = dialogueObject.GetGraph();
            if (graphView == null)
            {
                EditorGUILayout.LabelField("    ");
                EditorGUILayout.LabelField("No Dialogue seleted");
                return;
            }
                    
        }
        private void OnEnable()
        {
            OnSelectionChange();
        
        }
        private void OnDisable()
        {
            if (graphView != null)
                rootVisualElement.Remove(graphView);

            if (dialogueObject != null)
                EditorUtility.SetDirty(dialogueObject);
        }

        private void OnSelectionChange()
        {
            DialogueObject newObject = Selection.activeObject as DialogueObject;
            if (newObject != null)
            {
                dialogueObject = newObject;
                graphView = newObject.GetGraph();
                SetUpGraph();
                Repaint();
            }
        }
        private void SetUpGraph()
        {
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
            GenerateToolbar();
            Repaint();
        }

        [OnOpenAsset(1)]
        private static bool OpenDialogue(int instanceID, int line)
        {
            DialogueObject d = EditorUtility.InstanceIDToObject(instanceID) as DialogueObject;
            if ( d != null)
            {
                OpenGraphView();
                return true;
            }
            return false;
        }

        private void ConstructGraph()
        {
            graphView = new DialogueGraphView
            {
                name = "Dialogue Graph"
            };
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }
        private void GenerateToolbar()
        {
            if (toolbar != null)
                rootVisualElement.Remove(toolbar);
            toolbar = new Toolbar();
            var nodeCreateButton = new Button(() =>
            {
              graphView.CreateNode("Dialogue Node");
            });
            nodeCreateButton.text = "Create Node";
            toolbar.Add(nodeCreateButton);
            rootVisualElement.Add(toolbar);
        }
    }	
}
