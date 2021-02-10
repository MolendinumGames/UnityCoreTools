using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.Pooling;

namespace CoreTools.UI
{
    [CustomEditor(typeof(PoolManager))]
    public class PoolEditor : Editor
    {
        [MenuItem("MyTools/GetPoolManager")]
        public static void GetOrCreatePoolManager()
        {
            PoolManager pool = FindObjectOfType<PoolManager>();
            if (pool == null)
            {
                pool = new GameObject("PoolManager").AddComponent(typeof(PoolManager)) as PoolManager;
            }
            Selection.activeGameObject = pool.gameObject;
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty list = serializedObject.FindProperty("poolItems");
            EditorGUI.indentLevel++;
            // Display
            //EditorGUILayout.PropertyField(list, false); will display standard list (+new features)
            if (list.isExpanded)
            {
                //EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                for (int i = 0; i < list.arraySize; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
                    if (GUILayout.Button("X", miniButtonWidth))
                    {
                        list.DeleteArrayElementAtIndex(i);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button("Add Pool"))
                {
                    list.arraySize++;
                }
            }

            EditorGUI.indentLevel--;
            serializedObject.ApplyModifiedProperties();
        }
        private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);
    }
}

