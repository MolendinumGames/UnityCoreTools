/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using UnityEngine;
using UnityEditor;

namespace ProductivityTools.GameObjectFinder
{
    public class SwitcherTool : EditorWindow
    {
        Transform FirstTransform { get; set; }
        const string firstTransfLabel = "Swap:";

        Transform SecondTransform { get; set; }
        const string secondTransfLabel = "and:";

        bool willDrawConnection = true;
        bool WillDrawConnection
        {
            get => willDrawConnection;
            set
            {
                willDrawConnection = value;
                IssueSceneRepaint();
            }
        }

        private Vector2 scrollPos;
        private static readonly GUIContent Header = new GUIContent("Switcher");

        const float connectionBlobWidth = .5f;
        const float connectionLineWidth = 1f;
        readonly Color connectionColor = Color.red;

        [MenuItem("Tools/Switcher")]
        public static void OpenWindow()
        {
            SwitcherTool switcherWindow = (SwitcherTool)GetWindow(typeof(SwitcherTool));
            switcherWindow.titleContent = Header;
            switcherWindow.Show();
        }

        private void OnEnable()  => SceneView.duringSceneGui += DrawConnection;
        private void OnDisable() => SceneView.duringSceneGui -= DrawConnection;

        private void OnGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            DrawHeader();
            DrawConnectionToggle();
            DrawFirstTransformField();
            DrawSecondTransformField();
            DrawButtons();

            GUILayout.EndScrollView();
        }

        private void DrawHeader()
        {
            GUIStyle headerStyle = new GUIStyle("WhiteLargeLabel")
            {
                alignment = TextAnchor.MiddleCenter
            };
            EditorGUILayout.LabelField("Switch Tool", headerStyle);

            GUILayout.Label("Swap positions of two transforms.");
        }

        private void DrawConnectionToggle()
        {
            WillDrawConnection = EditorGUILayout.Toggle("Draw Connection: ", WillDrawConnection);
        }

        private void DrawFirstTransformField()
        {
            GUILayout.Label(firstTransfLabel, GUILayout.Width(EditorGUIUtility.currentViewWidth * .5f));
            Transform newFirstTransform = EditorGUILayout.ObjectField(FirstTransform, typeof(Transform), true) as Transform;
            if (newFirstTransform != FirstTransform && !EditorUtility.IsPersistent(newFirstTransform))
            {
                FirstTransform = newFirstTransform;
                IssueSceneRepaint();
            }
        }

        private void DrawSecondTransformField()
        {
            GUILayout.Label(secondTransfLabel, GUILayout.Width(EditorGUIUtility.currentViewWidth * .5f));
            Transform secondNewTransform = EditorGUILayout.ObjectField(SecondTransform, typeof(Transform), true) as Transform;
            if (secondNewTransform != SecondTransform && !EditorUtility.IsPersistent(secondNewTransform))
            {
                SecondTransform = secondNewTransform;
                IssueSceneRepaint();
            }
        }

        private void DrawButtons()
        {
            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(!SwapPossible());
            if (GUILayout.Button("Switch Positions"))
            {
                SwapPositions();
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Clear Fields"))
            {
                ClearTransformFields();
            }
            GUILayout.EndHorizontal();
        }

        private bool SwapPossible()
        {
            return FirstTransform != null
                && SecondTransform != null
                && FirstTransform != SecondTransform;
        }

        private void SwapPositions()
        {
            Vector3 firstPosition = FirstTransform.position;
            FirstTransform.position = SecondTransform.position;
            SecondTransform.position = firstPosition;
        }

        private void ClearTransformFields()
        {
            FirstTransform = null;
            SecondTransform = null;
            IssueSceneRepaint();
        }

        private void DrawConnection(SceneView sceneView)
        {
            if (!WillDrawConnection)
                return;

            Handles.color = connectionColor;

            bool firstTransformSelected  = FirstTransform != null;
            bool secondTransformSelected = SecondTransform != null;

            if (firstTransformSelected)
                DrawConnectionSphere(FirstTransform.position);

            if (secondTransformSelected)
                DrawConnectionSphere(SecondTransform.position);

            if (firstTransformSelected && secondTransformSelected)
                DrawConnectionLine(FirstTransform.position, SecondTransform.position);
        }

        private void DrawConnectionSphere(Vector3 position)
        {
            Handles.SphereHandleCap(0, position, Quaternion.identity, connectionBlobWidth, EventType.Repaint);
        }

        private void DrawConnectionLine(Vector3 start, Vector3 end)
        {
            Handles.DrawLine(start, end, connectionLineWidth);
        }

        private void IssueSceneRepaint() => SceneView.RepaintAll(); 
    }
}