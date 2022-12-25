/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using CoreTools;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace CoreTools.Pooling
{
    [CustomEditor(typeof(GlobalPoolManager))]
    public class GlobalPoolManagerInspector : UnityEditor.Editor
    {
        SerializedProperty poolsProp;
        UnityEditorInternal.ReorderableList poolList;

        const float ElementMargin = 4f;

        private void OnEnable()
        {
            StorePropertyReference();
            CreateReorderableList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            poolList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void StorePropertyReference() =>
            poolsProp = serializedObject.FindProperty("globalPools");

        private void CreateReorderableList()
        {
            poolList = new UnityEditorInternal.ReorderableList(serializedObject, poolsProp)
            {
                displayAdd = true,
                displayRemove = true,
                draggable = true,
                drawHeaderCallback = rect =>
                {
                    EditorGUI.LabelField(rect, "Global Pools");
                },
                onAddCallback = addcallback =>
                {
                    var manager = (GlobalPoolManager)serializedObject.targetObject;
                    manager.AddPool();
                },
                elementHeightCallback = index =>
                {
                    var element = poolsProp.GetArrayElementAtIndex(index);
                    float height = EditorGUI.GetPropertyHeight(element);
                    height += ElementMargin;
                    return height;
                },
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    var element = poolsProp.GetArrayElementAtIndex(index);
                    EditorGUI.PropertyField(rect, element);
                },
            };
        }
    }
}