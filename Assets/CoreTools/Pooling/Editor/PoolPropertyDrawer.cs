/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using UnityEngine;
using UnityEditor;
using CoreTools.Pooling;

namespace CoreTools.Pooling.CoreEditor
{
    [CustomPropertyDrawer(typeof(GameObjectPool))]
    sealed internal class PoolPropertyDrawer : PropertyDrawer
    {
        const float PropertyLineAmount = 3f;
        const int PropertyLabelWidth = 44;

        readonly SubPropertyInfo PrefabProperty = new("prefab", " Prefab");
        readonly SubPropertyInfo StartAmountProperty = new("startingAmount", "Start");
        readonly SubPropertyInfo MaxAmountProperty = new("maxAmount", " Max");
        readonly SubPropertyInfo ReuseProperty = new("reuseOnFull", "Reuse");

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

            EditorGUI.indentLevel = indent;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            //Previously: return base.GetPropertyHeight(property, label);
            return EditorGUIUtility.singleLineHeight * PropertyLineAmount;
        }

        private void DrawFirstRow(SerializedProperty property, Rect currentContentRect)
        {
            currentContentRect.width *= 0.5f;
            DrawSubProperty(currentContentRect, property, ReuseProperty);

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

        private void SetLabelWidth(int width) =>
            // 2 => extra padding
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
