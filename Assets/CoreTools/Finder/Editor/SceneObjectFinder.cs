using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace CoreTools.CoreEditor
{
    public class SceneObjectFinder : EditorWindow
    {
        
        string targetName = "";
        LayerMask layer;
        int selectedIndex = 0;
        Vector2 scrollPos = Vector2.zero;
        private static readonly GUIContent header = new GUIContent("GO Finder", "Find GameObjects in the hierarchy.");

        [MenuItem("Window/Finder")]
        public static void Init()
        {
            var window = GetWindow<SceneObjectFinder>();
            window.titleContent = header;
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginScrollView(scrollPos);
            DrawHeader();

            GUILayout.Label("By Tag: ");
            GUILayout.BeginHorizontal();
            string[] tagOptions = UnityEditorInternal.InternalEditorUtility.tags;
            selectedIndex = EditorGUILayout.Popup(selectedIndex, tagOptions, GUILayout.Width(.5f*EditorGUIUtility.currentViewWidth));
            if (GUILayout.Button("Find By Tag"))
            {
                var results = GameObject.FindGameObjectsWithTag(tagOptions[selectedIndex]);
                Selection.objects = results;
            }
            GUILayout.EndHorizontal();



            GUILayout.Label("Name Contains: ");
            GUILayout.BeginHorizontal();
            targetName = GUILayout.TextField(targetName, GUILayout.Width(EditorGUIUtility.currentViewWidth * .5f));
            if (GUILayout.Button("Find By Name"))
            {
                Selection.objects = GameObject.FindObjectsOfType<Transform>()
                    .Where(t => t.gameObject.name.Contains(targetName))
                    .Select( t => t.gameObject)
                    .ToArray();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }
        private void DrawHeader()
        {
            GUIStyle headerStyle = new GUIStyle("WhiteLargeLabel")
            {
                alignment = TextAnchor.MiddleCenter
            };
            EditorGUILayout.LabelField("GameObject Finder", headerStyle);
        }

        

    }
}