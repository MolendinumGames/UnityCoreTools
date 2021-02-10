using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SundirayTools
{
    public class ReplacerWindow : EditorWindow
    {
        bool moveToSelection = true;
        bool keepParents = true;
        bool dontDestory = false;
        bool applyRotation = false;
        bool applyScale = false;
        Vector3 offset = Vector3.zero;

        GameObject newGO;

        static GUIContent windowTitle = new GUIContent("Replacer Tool", "Replace selected GameObjects with a Prefab.");

        [MenuItem("MyTools/Replacer")]
        static void Init()
        {
            ReplacerWindow window = (ReplacerWindow)GetWindow(typeof(ReplacerWindow));
            window.titleContent = windowTitle;
            window.Show();
        }

        private void OnGUI()
        {
            DrawHeader();

            GUILayout.Label("Replace all GameObjects in your Selection with the target Prefab.");

            DrawSettings();

            EditorGUILayout.Space();

            newGO = EditorGUILayout.ObjectField("Replace With:", newGO, typeof(GameObject), false) as GameObject;

            EditorGUILayout.Space();

            DrawReplaceButton();
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
            offset = EditorGUILayout.Vector3Field("Offset All:", offset);
        }

        private void DrawReplaceButton()
        {
            EditorGUI.BeginDisabledGroup(PrefabCheck());
            if (GUILayout.Button("Replace"))
            {
                TryReplaceSelection();
            }
            EditorGUI.EndDisabledGroup();
        }
        private void TryReplaceSelection()
        {
            Transform[] selectedTransforms = Selection.GetTransforms(SelectionMode.OnlyUserModifiable | SelectionMode.Editable);
            if (selectedTransforms.Length == 0)
            {
                Debug.LogWarning("No editable GameObjects are selected!");
                return;
            }
            List<GameObject> newSelectedObjects = new List<GameObject>();
            foreach (var t in selectedTransforms)
            {
                GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(newGO);
                Undo.RegisterCreatedObjectUndo(go, "Created Replacement Prefabs");
                go.transform.position = t.position + offset;

                if (applyRotation)
                    go.transform.localRotation = t.transform.localRotation;

                if (applyScale)
                    go.transform.localScale = t.transform.localScale;

                if (keepParents && t.parent != null)
                    go.transform.SetParent(t.parent);

                if (!dontDestory)
                    Undo.DestroyObjectImmediate(t.gameObject);

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
                if (pref == PrefabAssetType.Regular | pref == PrefabAssetType.Model || pref == PrefabAssetType.Variant)
                    return false;
            }
            return true;
        }
    }
}
