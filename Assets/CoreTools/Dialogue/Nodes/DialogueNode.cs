using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.Dialogue
{
    public class DialogueNode : GraphNode
    {
        public Vector2 boxScroll = Vector2.zero;

        [SerializeField]
        private string text = "New Dialogue Text";
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
        private Sprite portrait;
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
        private string speaker;
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
        private DialogueOrientation orientation = DialogueOrientation.DownLeft;
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
        public override void Reset()
        {
            Undo.RecordObject(this, "Reset DialogueNode");
            speaker = null;
            portrait = null;
            ChildID = null;
            text = "";
            orientation = DialogueOrientation.DownLeft;
            EditorUtility.SetDirty(this);
        }
#endif
        public bool HasChild() => !string.IsNullOrEmpty(ChildID);
    }
}