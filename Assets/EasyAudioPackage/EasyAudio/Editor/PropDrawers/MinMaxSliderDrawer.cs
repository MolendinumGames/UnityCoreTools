using UnityEditor;
using UnityEngine;

namespace EasyAudioSystem.Utility
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        private const float textWidth = 0.4f;
        private const float numberWidth = .3f;

        private Sprite resetSprite;
        private GUIStyle resetStyle = new GUIStyle();
        private const string resetPath = "ResetButtonFinal5";
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            resetSprite = Resources.Load<Sprite>(resetPath);
            resetStyle.normal.background = resetSprite.texture;

            EditorGUI.BeginProperty(position, label, property);

            MinMaxSliderAttribute slider = attribute as MinMaxSliderAttribute;

            if (property.propertyType == SerializedPropertyType.Vector2)
            {
                var xValue = property.vector2Value.x;
                var yValue = property.vector2Value.y;

                int indent = EditorGUI.indentLevel;
                //EditorGUI.indentLevel = 1;
                float lineHeight = EditorGUIUtility.singleLineHeight;
                float restWidth = position.width - lineHeight * 2;
                Rect pos = position;

                // Label
                pos.height = lineHeight;
                pos.width = restWidth * textWidth;
                EditorGUI.LabelField(pos, property.name.FirstCharIsUpper());

                EditorGUI.BeginChangeCheck();

                // First float field
                pos.x += pos.width;
                pos.width = restWidth * numberWidth;
                float newX = EditorGUI.FloatField(pos, GUIContent.none, xValue);

                // Label "to"
                pos.x += pos.width;
                pos.width = lineHeight;
                EditorGUI.LabelField(pos, " to ");

                // Second float field
                pos.x += pos.width;
                pos.width = restWidth * numberWidth;
                float newY = EditorGUI.FloatField(pos, yValue);

                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = new Vector2(Mathf.Clamp(newX, slider.minValue, yValue),
                                                        Mathf.Clamp(newY, xValue, slider.maxValue));
                }

                // R Button
                pos.x += pos.width;
                pos.width = lineHeight;
                if (GUI.Button(pos, GUIContent.none, resetStyle))
                {
                    var minValX = Mathf.Clamp(slider.minReset, slider.minValue, slider.maxValue);
                    var minValY = Mathf.Clamp(slider.minReset, slider.minValue, slider.maxValue);
                    if (minValX > minValY)
                        minValX = minValY;
                    property.vector2Value = new Vector2(minValX, minValY);
                }

                // MinMaxSlider
                EditorGUI.BeginChangeCheck();
                EditorGUI.indentLevel += 2;
                pos.x = position.x;
                pos.y += lineHeight;
                pos.width = position.width;
                EditorGUI.MinMaxSlider(pos,
                                       ref xValue, ref yValue,
                                       slider.minValue, slider.maxValue);

                EditorGUI.indentLevel = indent;
                if (EditorGUI.EndChangeCheck())
                {
                    property.vector2Value = new Vector2(xValue, yValue);
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int rowCount = 2;
            return rowCount * EditorGUIUtility.singleLineHeight;
        }
    }
}
