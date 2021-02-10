using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EasyAudioSystem;
using EasyAudioSystem.Utility;

namespace EasyAudioSystem.UI
{
    /* This class determines how the property of 'EasyAudio' is being drawn in your inspector.
     * It will not be included in the final build of your project.
     * The property will only need a mandatory field to plug in the data holding EasyAudioObj.
     * The AudioSource field can be left null as long as a target AudioSource is being passed in via script.
     * For more information on the EasyAudio workflow please check out the documentation:
     * 
     * For more information on PropertyDrawers in Unity check out the Unity Documentation:
     * https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
     */

    /// <summary>
    /// Property Drawer for 'EasyAudio'. Handles play functionality and holds reference to an EasyAudioObj.
    /// </summary>
    [CustomPropertyDrawer(typeof(EasyAudio))]
    public class EasyAudioPlayerDrawer : PropertyDrawer
    {
        // Label Data
        //GUIContent header = new GUIContent("  EasyAudio", "Needs a EasyAudio ScriptableObject to funcntion.");
        GUIContent objLabel = new GUIContent("State Data", "The EasyAudio ScriptableObject to get the clip data from.");
        GUIContent sourceLabel = new GUIContent("AudioSource", "AudioSource can be null if set in script.");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw Background
            GUIStyle style = new GUIStyle("ProjectBrowserTextureIconDropShadow");
            if (Event.current.type == EventType.Repaint)
                style.Draw(position, GUIContent.none, 0);

            // Set up resuable field rect based on property position
            Rect newPos = position;
            newPos.y += 2;
            newPos.x += 8;
            newPos.width *= .96f;
            newPos.height = EditorGUIUtility.singleLineHeight;

            // Store properties
            var easyAudioProp = property.FindPropertyRelative("easyAudio");
            var audioSourceProp = property.FindPropertyRelative("audioSource");

            // Draw Header
            EditorGUI.LabelField(newPos, property.name.CamelCaseToDescription(), EditorStyles.boldLabel);

            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel++;

            // Draw object fields
            newPos.y += EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.PropertyField(newPos, easyAudioProp, objLabel);
            newPos.y += EditorGUIUtility.singleLineHeight + 2;
            EditorGUI.PropertyField(newPos, audioSourceProp, sourceLabel);

            //Reset Indent
            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 3;
            int extraPixels = 13;
            return EditorGUIUtility.singleLineHeight * totalLines + extraPixels;
        }
    }
}

