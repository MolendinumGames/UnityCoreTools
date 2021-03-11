using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

    // For support, feedback and suggestions please conact me under:
    // contactsundiray@gmail.com
    // Check out my other content:
    // https://sundiray.itch.io/

namespace CoreTools.Replacer
{
    public class ReplacerWindow : EditorWindow
    {
        // target Prefab
        private GameObject newGO;

        // Viewport
        private Vector2 scrollPos = Vector2.zero;
        static readonly GUIContent windowTitle = new GUIContent("Replacer Tool", 
                                                                "Replace selected GameObjects with a Prefab.");

        // Parameters
        private bool moveToSelection = true;
        private bool keepParents = true;
        private bool dontDestory = false;
        private bool applyRotation = false;
        private bool applyScale = false;
        private bool applyTag = false;
        private Vector3 offset = Vector3.zero;

        [MenuItem("Window/Replacer")]
        static void Init()
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

            GUILayout.Label("Replace selected GameObjects with a Prefab.");

            DrawSettings();

            EditorGUILayout.Space();

            newGO = EditorGUILayout.ObjectField("Replace With:", newGO, typeof(GameObject), false) as GameObject;

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
        }
        private void DrawSettings()
        {
            moveToSelection = EditorGUILayout.Toggle("Auto Select New:", moveToSelection);
            keepParents = EditorGUILayout.Toggle("Keep Parent:", keepParents);
            dontDestory = EditorGUILayout.Toggle("Don't Destroy Old", dontDestory);
            applyRotation = EditorGUILayout.Toggle("Keep Rotation:", applyRotation);
            applyScale = EditorGUILayout.Toggle("Keep Scale:", applyScale);
            applyTag = EditorGUILayout.Toggle("Keep Tag: ", applyTag);
            offset = EditorGUILayout.Vector3Field("Offset All:", offset);
        }

        private void DrawReplaceButton()
        {
            var targets = GetSelectedSceneGameObjects();

            EditorGUI.BeginDisabledGroup(PrefabCheck() || targets.Count == 0);
            string buttonTag = "Replace " + targets.Count.ToString();
            if (GUILayout.Button(buttonTag))
            {
                TryReplaceSelection();
            }
            EditorGUI.EndDisabledGroup();
        }
        private void TryReplaceSelection()
        {
            var selection = GetSelectedSceneGameObjects();
            if (selection.Count == 0)
            {
                Debug.LogWarning("No editable GameObjects are selected!");
                return;
            }
            List<GameObject> newSelectedObjects = new List<GameObject>();
            foreach (var target in selection)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(newGO);
                Undo.RegisterCreatedObjectUndo(go, "Created Replacement Prefabs");
                go.transform.position = target.transform.position + offset;

                if (applyRotation)
                    go.transform.localRotation = target.transform.localRotation;

                if (applyScale)
                    go.transform.localScale = target.transform.localScale;

                if (keepParents && target.transform.parent != null)
                    go.transform.SetParent(target.transform.parent);

                if (applyTag)
                    go.tag = target.tag;

                if (!dontDestory)
                    Undo.DestroyObjectImmediate(target);

                if (moveToSelection)
                    newSelectedObjects.Add(go);
            }
            if (moveToSelection)
                Selection.objects = newSelectedObjects.ToArray();
        }
        private bool PrefabCheck()
        {
            if (newGO != null)
            {
                PrefabAssetType pref = PrefabUtility.GetPrefabAssetType(newGO);
                if (pref == PrefabAssetType.Regular || pref == PrefabAssetType.Model || pref == PrefabAssetType.Variant)
                    return false;
            }
            return true;
        }
        private List<GameObject> GetSelectedSceneGameObjects()
        {
            return Selection.GetTransforms(SelectionMode.OnlyUserModifiable | SelectionMode.Editable)
                    .Where(t => t.gameObject.scene.IsValid())
                    .Select(t => t.gameObject)
                    .ToList();
        }
    }
}
