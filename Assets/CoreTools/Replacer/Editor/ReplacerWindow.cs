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

namespace CoreTools.GameObjectFinder
{
    public class ReplacerWindow : EditorWindow
    {
        GameObject TargetNewObject { get; set; }

        private Vector2 scrollPos = Vector2.zero;
        static readonly GUIContent windowTitle = new GUIContent("Replacer Tool", 
                                                                "Replace selected GameObjects with a Prefab.");

        bool WillSelect { get; set; } = true;
        bool WillKeepParent { get; set; } = true;
        bool WontDestroyOld { get; set; } = false;
        bool AppliesRotation { get; set; } = false;
        bool AppliesScale { get; set; } = false;
        bool AppliesTag { get; set; } = false;
        Vector3 OffsetVector { get; set; } = Vector3.zero;

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

            DrawSettings();

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
            EditorGUILayout.LabelField("Replacer Tool", headerStyle);
            GUILayout.Label("Replace selected GameObjects with a Prefab.");
        }

        private void DrawSettings()
        {
            WillSelect = EditorGUILayout.Toggle("Auto Select New:", WillSelect);
            WillKeepParent = EditorGUILayout.Toggle("Keep Parent:", WillKeepParent);
            WontDestroyOld = EditorGUILayout.Toggle("Don't Destroy Old", WontDestroyOld);
            AppliesRotation = EditorGUILayout.Toggle("Keep Rotation:", AppliesRotation);
            AppliesScale = EditorGUILayout.Toggle("Keep Scale:", AppliesScale);
            AppliesTag = EditorGUILayout.Toggle("Keep Tag:", AppliesTag);
            OffsetVector = EditorGUILayout.Vector3Field("Offset All:", OffsetVector);
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

            if (WillSelect)
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
                    return false;
            }

            return true;
        }
        private GameObject[] GetSelectedSceneGameObjects()
        {
            return Selection.GetTransforms(SelectionMode.OnlyUserModifiable | SelectionMode.Editable)
                            .Where(t => t.gameObject.scene.IsValid())
                            .Select(t => t.gameObject)
                            .ToArray();
        }
    }
}
