using UnityEngine;
using UnityEditor;
using CoreTools;

namespace CoreTools.Pooling
{
    [CustomPropertyDrawer(typeof(PoolItem))]
    public class PoolItemDrawer : PropertyDrawer
    {
        static readonly Color darkGrey = new Color(.17f, .17f, .17f, 1f);

        const float PropertyLineAmount = 3.5f;
        const int PropertyLabelWidth = 44;

        struct SubPropertyInfo
        {
            public string PropertyName;
            public string Description;

            public SubPropertyInfo(string propertyName, string description)
            {
                this.PropertyName = propertyName;
                this.Description = description;
            }
        }

        static readonly SubPropertyInfo KeyProperty             = new SubPropertyInfo("key", "Key");
        static readonly SubPropertyInfo PrefabProperty          = new SubPropertyInfo("prefab", " Prefab");
        static readonly SubPropertyInfo StartAmountProperty     = new SubPropertyInfo("startingAmount", "Start");
        static readonly SubPropertyInfo MaxAmountProperty       = new SubPropertyInfo("maxAmount", " Max");
        static readonly SubPropertyInfo CreateOnStartProperty   = new SubPropertyInfo("createOnStart", "Load on Start");
        static readonly SubPropertyInfo ReuseProperty           = new SubPropertyInfo("reuseOnFull", " Reuse");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Store inital indent then draw the property without indenting
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            SetLabelWidth(PropertyLabelWidth);
            Rect contentPos = position;
            contentPos.height /= 3;

            DrawFirstRow(property, contentPos);
            NextLine(ref contentPos);

            DrawSecondRow(property, contentPos);
            NextLine(ref contentPos);

            DrawThirdRow(property, contentPos);
            NextLine(ref contentPos);

            DrawLine(contentPos);

            EditorGUI.indentLevel = indent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //Previously: return base.GetPropertyHeight(property, label);
            return EditorGUIUtility.singleLineHeight * PropertyLineAmount;
        }

        private void DrawLine(Rect currentContentRect)
        {
            Rect lineRect = currentContentRect;
            lineRect.height = 2f;
            EditorGUI.DrawRect(lineRect, darkGrey);
        }

        private void DrawFirstRow(SerializedProperty property, Rect currentContentRect)
        {
            currentContentRect.width *= 0.5f;
            DrawSubProperty(currentContentRect, property, KeyProperty);

            currentContentRect.x += currentContentRect.width;
            DrawSubProperty(currentContentRect, property, PrefabProperty);
        }

        private void DrawSecondRow(SerializedProperty property, Rect currentContentRect)
        {
            currentContentRect.width *= 0.5f;
            DrawSubProperty(currentContentRect, property, StartAmountProperty);

            currentContentRect.x += currentContentRect.width;
            DrawSubProperty(currentContentRect, property, MaxAmountProperty);
        }

        private void DrawThirdRow(SerializedProperty property, Rect currentContentRect)
        {
            EditorGUIUtility.labelWidth *= 2f;

            currentContentRect.width *= 0.5f;
            DrawSubProperty(currentContentRect, property, CreateOnStartProperty);

            currentContentRect.x += currentContentRect.width;
            DrawSubProperty(currentContentRect, property, ReuseProperty);

            EditorGUIUtility.labelWidth *= .5f;
        }

        private void SetLabelWidth(int width) =>
            EditorGUIUtility.labelWidth = width;

        private void NextLine(ref Rect contentPos) =>
            contentPos.y += EditorGUIUtility.singleLineHeight + 2;

        private void DrawSubProperty(Rect contentRect, SerializedProperty property, SubPropertyInfo propInfo) =>
            EditorGUI.PropertyField(
                contentRect, 
                property.FindPropertyRelative(propInfo.PropertyName), 
                new GUIContent(propInfo.Description));
    }
}
