/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProductivityTools.GameObjectFinder
{
    public class ReplacerWindow : EditorWindow
    {
        GameObject TargetNewObject { get; set; }

        private Vector2 scrollPos = Vector2.zero;
        static readonly GUIContent windowTitle = new GUIContent("Replacer Tool", 
                                                                "Replace selected GameObjects with a Prefab.");

        bool WillSelectNew { get; set; }    = true;
        const string WillSelectNewMessage   = "Auto select new:";

        bool WillKeepParent { get; set; }   = true;
        const string WillKeepParentMessage  = "Keep Parent:";

        bool WontDestroyOld { get; set; }   = false;
        const string WontDestroyOldMessage  = "Don't destroy old:";

        bool AppliesRotation { get; set; }  = false;
        const string ApplyRotationMessage   = "Keep rotation:";

        bool AppliesScale { get; set; }     = false;
        const string ApplyScaleMessage      = "Keep local scale:";

        bool AppliesTag { get; set; }       = false;
        const string ApplyTagMessage        = "Keep Tag:";

        Vector3 OffsetVector { get; set; }  = Vector3.zero;
        const string OffsetMessage          = "Offset new by:";

        [MenuItem("Tools/Replacer")]
        public static void OpenReplacerWindow()
        {
            ReplacerWindow window = (ReplacerWindow)GetWindow(typeof(ReplacerWindow));
            window.titleContent = windowTitle;
            window.Show();
        }

        private void OnSelectionChange() => Repaint();
        
        private void OnGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            DrawHeader();

            DrawAndReadSettings();

            EditorGUILayout.Space();

            DrawTargetObjectField();

            EditorGUILayout.Space();

            DrawReplaceButton();

            GUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            GUIStyle headerStyle = new GUIStyle("WhiteLargeLabel")
            {
                alignment = TextAnchor.MiddleCenter
            };
            EditorGUILayout.LabelField(windowTitle.text, headerStyle);
            GUILayout.Label(windowTitle.tooltip);
        }

        private void DrawAndReadSettings()
        {
            WillSelectNew   = EditorGUILayout.Toggle(WillSelectNewMessage, WillSelectNew);
            WillKeepParent  = EditorGUILayout.Toggle(WillKeepParentMessage, WillKeepParent);
            WontDestroyOld  = EditorGUILayout.Toggle(WontDestroyOldMessage, WontDestroyOld);
            AppliesRotation = EditorGUILayout.Toggle(ApplyRotationMessage, AppliesRotation);
            AppliesScale    = EditorGUILayout.Toggle(ApplyScaleMessage, AppliesScale);
            AppliesTag      = EditorGUILayout.Toggle(ApplyTagMessage, AppliesTag);
            OffsetVector    = EditorGUILayout.Vector3Field(OffsetMessage, OffsetVector);
        }

        private void DrawTargetObjectField()
        {
            TargetNewObject = EditorGUILayout.ObjectField("Replace With:", TargetNewObject, typeof(GameObject), false) as GameObject;
        }

        private void DrawReplaceButton()
        {
            GameObject[] selectedGameObjects = GetSelectedSceneGameObjects();

            EditorGUI.BeginDisabledGroup(!ValidTargetedPrefab() || selectedGameObjects.Length == 0);
            string buttonTag = $"Replace {selectedGameObjects.Length} Objects";
            if (GUILayout.Button(buttonTag))
            {
                TryReplaceSelection();
            }
            EditorGUI.EndDisabledGroup();
        }

        private void TryReplaceSelection()
        {
            GameObject[] selection = GetSelectedSceneGameObjects();
            if (selection.Length == 0)
            {
                Debug.LogWarning("No editable GameObjects are selected!");
            }
            else
            {
                ReplaceSelectedGameObjects(selection);
            }
        }

        private void ReplaceSelectedGameObjects(GameObject[] selection)
        {
            List<GameObject> newCreatedObjects = new List<GameObject>();
            foreach (var targetToReplace in selection)
            {
                newCreatedObjects.Add(CreateAndSetupNewGameObject(targetToReplace));
            }

            if (WillSelectNew)
                Selection.objects = newCreatedObjects.ToArray();
        }

        private GameObject CreateAndSetupNewGameObject(GameObject targetToReplace)
        {
            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(TargetNewObject);
            Undo.RegisterCreatedObjectUndo(go, "Created Replacement Prefabs");
            go.transform.position = targetToReplace.transform.position + OffsetVector;

            if (AppliesRotation)
                go.transform.rotation = targetToReplace.transform.rotation;

            if (WillKeepParent && targetToReplace.transform.parent != null)
                go.transform.SetParent(targetToReplace.transform.parent);

            if (AppliesScale)
                go.transform.localScale = targetToReplace.transform.localScale;

            if (AppliesTag)
                go.tag = targetToReplace.tag;

            if (!WontDestroyOld)
                Undo.DestroyObjectImmediate(targetToReplace);

            return go;
        }

        private bool ValidTargetedPrefab()
        {
            if (TargetNewObject != null)
            {
                PrefabAssetType pref = PrefabUtility.GetPrefabAssetType(TargetNewObject);
                if (pref == PrefabAssetType.Regular || pref == PrefabAssetType.Model || pref == PrefabAssetType.Variant)
                    return true;
            }

            return false;
        }
        private GameObject[] GetSelectedSceneGameObjects()
        {
            return Selection.GetTransforms(SelectionMode.Editable)
                            .Where(t => t.gameObject.scene.IsValid())
                            .Select(t => t.gameObject)
                            .ToArray();
        }
    }
}
