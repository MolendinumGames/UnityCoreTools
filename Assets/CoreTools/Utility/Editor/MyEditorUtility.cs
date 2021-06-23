using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools
{
	public static class MyEditorUtility
	{
		public static void DrawLine(float height, Color color, float padding, int indent)
        {
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = indent;

            EditorGUILayout.Space();
            Rect rect = EditorGUILayout.GetControlRect(false, 2 * padding);
            rect.height = height;
            rect.width *= 2;
            rect.position = new Vector2(0, rect.position.y);
            EditorGUI.DrawRect(rect, color);

            EditorGUI.indentLevel = oldIndent;
        }
    }	
}
