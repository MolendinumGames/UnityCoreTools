using UnityEngine;
using UnityEditor;

namespace EasyAudioSystem.Utility
{
    [CustomPropertyDrawer(typeof(NoEditAttribute))]
    public class NoEditDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            UnityEngine.GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            UnityEngine.GUI.enabled = true;
        }
    }
}
