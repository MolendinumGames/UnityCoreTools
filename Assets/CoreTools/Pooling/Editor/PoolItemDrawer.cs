using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.Pooling;

namespace CoreTools.UI
{
    [CustomPropertyDrawer(typeof(PoolItem))]
    public class PoolItemDrawer : PropertyDrawer
    {
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

            EditorGUI.indentLevel = indent;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //return base.GetPropertyHeight(property, label) *2;
            //return 40;
            int rowCount = 3;
            return EditorGUIUtility.singleLineHeight * rowCount;
        }
    }
}
