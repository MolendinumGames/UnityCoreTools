/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem
{
    public class DialogueNode : GraphNode
    {
        public override bool IsEntry => false;

        [SerializeField]
        protected string text = "New Dialogue Text";
        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set
            {
                if (text != value)
                {
                    Undo.RecordObject(this, "Update Node Text");
                    text = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }

        [SerializeField]
        protected Sprite portrait;
        public Sprite Portrait
        {
            get => portrait;
#if UNITY_EDITOR
            set
            {
                if (portrait != value)
                {
                    Undo.RecordObject(this, "Changed node portrait");
                    portrait = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }

        [SerializeField]
        protected string speaker;
        public string Speaker
        {
            get => speaker;
#if UNITY_EDITOR
            set
            {
                if (speaker != value)
                {
                    Undo.RecordObject(this, "Changed node speaker");
                    speaker = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }

        [SerializeField]
        protected DialogueOrientation orientation = DialogueOrientation.Left;
        public DialogueOrientation Orientation
        {
            get => orientation;
            set
            {
                if (orientation != value)
                {
                    Undo.RecordObject(this, "Changed Node Orientation");
                    orientation = value;
                    EditorUtility.SetDirty(this);
                }
            }
        }


#if UNITY_EDITOR
        [SerializeField]
        protected Rect rect = new Rect(10, 10, 300, 180);
        public override Rect NodeRect { get => rect; set => rect = value; }
        public override Rect GetBaseRect() => rect;
        public override void Reset()
        {
            Undo.RecordObject(this, "Reset DialogueNode");
            speaker = null;
            portrait = null;
            text = "";
            orientation = DialogueOrientation.Left;
            OnReset();
            EditorUtility.SetDirty(this);
        }
        protected virtual void OnReset() { }
        public Vector2 boxScroll = Vector2.zero;
#endif
    }
}