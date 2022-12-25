/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using System.Linq;
using UnityEngine;
using UnityEditor;

namespace ProductivityTools.GameObjectFinder
{
    public class GameObjectFinder : EditorWindow
    {
        string TargetName { get; set; } = "";
        int SelectedIndex { get; set; } = 0;
        string[] TagOptions
        {
            get => UnityEditorInternal.InternalEditorUtility.tags;
        }

        private Vector2 scrollPos = Vector2.zero;
        private static readonly GUIContent Header = new GUIContent("GO Finder", "Find GameObjects in the hierarchy.");

        [MenuItem("Tools/Finder")]
        public static void OpenFinderWindow()
        {
            var window = GetWindow<GameObjectFinder>();
            window.titleContent = Header;
            window.Show();
        }

        private void OnGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            DrawHeader();

            DrawFindByTagSection();

            DrawFindByNameSection();

            GUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            GUIStyle headerStyle = new GUIStyle("WhiteLargeLabel")
            {
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label("GameObject Finder", headerStyle);

            GUILayout.Label("Search scene hierarchy for GameObejcts.");
        }

        #region Find By Tag
        private void DrawFindByTagSection()
        {
            GUILayout.Label("By Tag: ");
            GUILayout.BeginHorizontal();
            DrawTagSelection();
            string selectedTag = TagOptions[SelectedIndex];
            DrawFindByTagButton(selectedTag);
            GUILayout.EndHorizontal();
        }

        private void DrawTagSelection()
        {
            int newSelectedIndex = EditorGUILayout.Popup(
                SelectedIndex, 
                TagOptions, 
                GUILayout.Width(.5f * EditorGUIUtility.currentViewWidth));
            SelectedIndex = newSelectedIndex;
        }

        private void DrawFindByTagButton(string selectedTag)
        {
            if (GUILayout.Button("Find By Tag"))
            {
                FindAndSelectGameObjectsByTag(selectedTag);
            }
        }

        private void FindAndSelectGameObjectsByTag(string selectedTag)
        {
            var foundObjects = GameObject.FindGameObjectsWithTag(selectedTag);
            Selection.objects = foundObjects;
        }
        #endregion

        #region Find By Name
        private void DrawFindByNameSection()
        {
            GUILayout.Label("Name Contains: ");
            GUILayout.BeginHorizontal();
            DrawTargetNameField();
            DrawFindByNameButton();
            GUILayout.EndHorizontal();
        }

        private void DrawTargetNameField()
        {
            string newName = GUILayout.TextField(
                TargetName,
                GUILayout.Width(EditorGUIUtility.currentViewWidth * .5f));
            TargetName = newName;
        }

        private void DrawFindByNameButton()
        {
            if (GUILayout.Button("Find By Name"))
            {
                FindAndSelectGameObjectByName(TargetName);
            }
        }

        private void FindAndSelectGameObjectByName(string gameObjectName)
        {
            Selection.objects = GameObject.FindObjectsOfType<Transform>()
                                          .Where(t => t.gameObject.name.Contains(gameObjectName))
                                          .Select(t => t.gameObject)
                                          .ToArray();
        }
        #endregion
    }
}