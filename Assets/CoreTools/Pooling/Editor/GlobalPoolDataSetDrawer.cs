using UnityEngine;
using UnityEditor;
using CoreTools.Pooling;

namespace CoreTools.Pooling.Editor
{
    [CustomPropertyDrawer(typeof(GlobalPoolDataSet))]
    sealed internal class GlobalPoolDataSetDrawer : PropertyDrawer
    {
        static readonly Color darkGrey = new Color(.17f, .17f, .17f, 1f);

        const float PropertyLineAmount = 3.7f;
        const int PropertyLabelWidth = 44;

        readonly SubPropertyInfo keyProperty = new ("key", "Key");
        readonly SubPropertyInfo poolProperty = new("Pool", "Pool");
        readonly SubPropertyInfo createOnStartProperty = new ("populateOnAwake", " Init on Awake");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Store inital indent then draw the property without indenting
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            SetLabelWidth(PropertyLabelWidth);
            Rect contentPos = position;
            contentPos.height = EditorGUIUtility.singleLineHeight;

            DrawFirstRow(property, contentPos);
            NextLine(ref contentPos);

            DrawSecondRow(property, contentPos);
            NextLine(ref contentPos);
            NextLine(ref contentPos);

            DrawLine(contentPos);

            EditorGUI.indentLevel = indent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Previously: return base.GetPropertyHeight(property, label);
            return EditorGUIUtility.singleLineHeight * PropertyLineAmount;
        }

        private void DrawLine(Rect currentContentRect)
        {
            // Setup line as rect
            Rect lineRect = currentContentRect;
            lineRect.height = 2f;
            lineRect.y += 2f;

            EditorGUI.DrawRect(lineRect, darkGrey);
        }

        private void DrawFirstRow(SerializedProperty property, Rect currentContentRect)
        {
            currentContentRect.width *= 0.5f;
            DrawSubProperty(currentContentRect, property, keyProperty);

            // Widen the Label
            float oldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth *= 2;
            currentContentRect.x += currentContentRect.width;

            DrawSubProperty(currentContentRect, property, createOnStartProperty);

            // Reset the previously widened label width
            EditorGUIUtility.labelWidth = oldWidth;
        }

        private void DrawSecondRow(SerializedProperty property, Rect currentContentRect)
        {
            DrawSubProperty(currentContentRect, property, poolProperty);
        }

        private void SetLabelWidth(int width) =>
            EditorGUIUtility.labelWidth = width;

        private void NextLine(ref Rect contentPos) =>
            // 2 => extra padding
            contentPos.y += EditorGUIUtility.singleLineHeight + 2;

        private void DrawSubProperty(Rect contentRect, SerializedProperty property, SubPropertyInfo propInfo) =>
            EditorGUI.PropertyField(
                contentRect,
                property.FindPropertyRelative(propInfo.PropertyName),
                new GUIContent(propInfo.Description));
    }
}
