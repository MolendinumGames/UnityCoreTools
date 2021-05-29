using UnityEngine;
using UnityEditor;

namespace CoreTools.Pooling
{
    [CustomPropertyDrawer(typeof(PoolItem))]
    public class PoolItemDrawer : PropertyDrawer
    {
        private readonly Color darkGrey = new Color(.17f, .17f, .17f, 1f);
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 44;
            Rect contentPos = position;
            contentPos.height /= 3;
            contentPos.width *= .5f;
            EditorGUI.PropertyField(contentPos, property.FindPropertyRelative("key"), new GUIContent("Key"));
            contentPos.x += contentPos.width;
            EditorGUI.PropertyField(contentPos, property.FindPropertyRelative("prefab"), new GUIContent(" Prefab"));
            //Second Row
            contentPos.y += EditorGUIUtility.singleLineHeight + 2;
            contentPos.x -= contentPos.width;
            EditorGUI.PropertyField(contentPos, property.FindPropertyRelative("startingAmount"), new GUIContent("Start"));
            contentPos.x += contentPos.width;
            EditorGUI.PropertyField(contentPos, property.FindPropertyRelative("maxAmount"), new GUIContent(" Limit"));
            contentPos.y += EditorGUIUtility.singleLineHeight + 2;
            contentPos.x -= contentPos.width;
            EditorGUIUtility.labelWidth *= 2f;
            EditorGUI.PropertyField(contentPos, property.FindPropertyRelative("createOnStart"), new GUIContent(" Load On Start"));
            EditorGUIUtility.labelWidth *= .5f;
            contentPos.y += EditorGUIUtility.singleLineHeight + 2;
            Rect lineRect = contentPos;
            lineRect.height = 2f;
            lineRect.width *= 2;
            EditorGUI.DrawRect(lineRect, darkGrey);

            EditorGUI.indentLevel = indent;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //return base.GetPropertyHeight(property, label) *2;
            return EditorGUIUtility.singleLineHeight * 3.5f;
        }
    }
}
