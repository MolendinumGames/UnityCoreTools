using UnityEngine;
using UnityEditor;
using EasyAudioSystem.Core;
using EasyAudioSystem.Utility;

namespace EasyAudioSystem.UI
{
    /* For more information on PropertyDrawers check out the corresponding Unity Documentation:
     * https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
     */
    /// <summary>
    /// Handles drawing of the EasyAudioShell in the inspector GUI.
    /// </summary>
    [CustomPropertyDrawer(typeof(EasyAudioShell))]
    public class EasyAudioShellDrawer : PropertyDrawer
    {
        private readonly string resetImagePath = "ResetButtonFinal5";
        private Sprite resetSprite;
        private GUIStyle resetStyle = new GUIStyle();

        private const float textWidth = 0.28f;
        private const float rangeWidth = 0.65f;
        private float lineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            LoadResetButtonStyle();

            Rect pos = EditorGUI.PrefixLabel(position, GUIContent.none);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;

            lineHeight = EditorGUIUtility.singleLineHeight;

            bool isExtended = property.FindPropertyRelative("isExtended").boolValue;
            //pos.height /= (isExtended ? 5 : 2);
            pos.height = lineHeight;

            EditorGUI.PropertyField(pos, property.FindPropertyRelative("clip"), GUIContent.none);

            pos.y += lineHeight;
            EditorGUI.indentLevel += 1;
            isExtended = EditorGUI.Foldout(pos, isExtended, "Clip Propeties");
            if (isExtended)
            {
                GUI.changed = true;

                pos.y += lineHeight;
                DrawFloatValue(property, pos, "volume", 1f);

                pos.y += lineHeight;
                DrawFloatValue(property, pos, "pitch", 2f);

                pos.y += lineHeight;
                DrawClipLength(pos, property);

            }
            property.FindPropertyRelative("isExtended").boolValue = isExtended;

            EditorGUI.EndProperty();
            EditorGUI.indentLevel = indent;
        }
        private void DrawFloatValue(SerializedProperty property, Rect pos, string name, float maxVal)
        {
            EditorGUI.LabelField(new Rect(pos.x, pos.y, pos.width * textWidth, lineHeight),
                                 property.FindPropertyRelative(name).name.FirstCharIsUpper());
            EditorGUI.Slider(new Rect(pos.x + pos.width * textWidth, pos.y, pos.width * rangeWidth, lineHeight),
                            property.FindPropertyRelative(name),
                            0, maxVal, GUIContent.none);
            if (GUI.Button(new Rect(pos.x + (textWidth + rangeWidth) * pos.width, pos.y, lineHeight, lineHeight), GUIContent.none, resetStyle))
            {
                property.FindPropertyRelative(name).floatValue = 1f;
                GUI.changed = true;
            }
        }
        void DrawClipLength(Rect pos, SerializedProperty p)
        {
            string newLabel = "Clip Length: ";
            AudioClip targetClip = p.FindPropertyRelative("clip").objectReferenceValue as AudioClip;

            if (targetClip != null)
                newLabel += targetClip.length.ToString() + " seconds";
            else
                newLabel += "No Clip referenced.";

            EditorGUI.LabelField(pos, newLabel);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * (property.FindPropertyRelative("isExtended").boolValue ? 5 : 2);
        }
        private void LoadResetButtonStyle()
        {
            resetSprite = Resources.Load<Sprite>(resetImagePath);
            resetStyle.normal.background = resetSprite.texture;
        }
    }
}
