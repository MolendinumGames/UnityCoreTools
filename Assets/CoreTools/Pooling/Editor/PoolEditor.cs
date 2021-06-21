using UnityEngine;
using UnityEditor;

namespace CoreTools.Pooling
{
    // !!! DEPRICATED !!!
    // [CustomEditor(typeof(PoolManager))]
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

            SerializedProperty list = serializedObject.FindProperty("poolData");
            EditorGUI.indentLevel++;
            // Display
            if (list.isExpanded)
            {
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

