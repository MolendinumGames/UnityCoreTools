using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// For support, feedback and suggestions please conact me under:
// contactsundiray@gmail.com
// Check out my other content:
// https://sundiray.itch.io/

namespace SundirayTools
{
    public class SwitcherTool : EditorWindow
    {
        Transform firstT;
        Transform secondT;
        bool drawGiz = true;
        Vector2 scrollPos;

        private static GUIContent windowTitle = new GUIContent("Switcher");
        [MenuItem("MyTools/Switcher")]
        public static void OpenWindow()
        {
            SwitcherTool switcherWindow = (SwitcherTool)GetWindow(typeof(SwitcherTool));
            switcherWindow.titleContent = windowTitle;
            switcherWindow.Show();
        }
        private void OnEnable() => SceneView.duringSceneGui += DrawConnection;
        private void OnDisable() => SceneView.duringSceneGui -= DrawConnection;

        private void OnGUI()
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            drawGiz = EditorGUILayout.Toggle("Draw Connection: ", drawGiz);

            DrawTransformField(ref firstT);
            DrawTransformField(ref secondT);

            GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(firstT == null || secondT == null || firstT == secondT);
            if (GUILayout.Button("Switch Positions"))
            {
                SwapPositions();
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Clear Fields"))
            {
                firstT = null;
                secondT = null;
                SceneView.RepaintAll();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }

        private void DrawTransformField(ref Transform t)
        {
            Transform newT = EditorGUILayout.ObjectField(t, typeof(Transform), true) as Transform;
            if (newT != t)
            {
                if (newT == null)
                {
                    t = null;
                }
                else if (!EditorUtility.IsPersistent(newT))
                {
                    t = newT;
                    SceneView.RepaintAll();
                }

            }
        }
        private void SwapPositions()
        {
            Vector3 first = firstT.position;
            Vector3 second = secondT.position;
            firstT.position = second;
            secondT.position = first;
        }
        private void DrawConnection(SceneView sceneView)
        {
            if (!drawGiz)
                return;

            Handles.color = Color.red;

            if (firstT != null)
                Handles.SphereHandleCap(0, firstT.position, Quaternion.identity, .5f, EventType.Repaint);

            if (secondT != null)
                Handles.SphereHandleCap(0, secondT.position, Quaternion.identity, .5f, EventType.Repaint);

            if (firstT != null && secondT != null)
                Handles.DrawLine(firstT.position, secondT.position);
        }
    }

}