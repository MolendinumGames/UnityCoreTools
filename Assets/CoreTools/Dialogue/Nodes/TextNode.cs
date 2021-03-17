using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem
{
    public class TextNode : DialogueNode, ISingleChild
    {

        [SerializeField]
        private string childID;
        public virtual string ChildID
        {
            get => childID;
#if UNITY_EDITOR
            set
            {
                if (childID != value)
                {
                    Undo.RecordObject(this, "Changed Dialogue Node childID");
                    childID = value;
                    EditorUtility.SetDirty(this);
                }
            }
#endif
        }
        public virtual bool HasChild() => !string.IsNullOrWhiteSpace(childID);
        public virtual void ClearChild()
        {
            ChildID = null;
        }
        public virtual void ClearChild(string id)
        {
            if (ChildID == id)
                ClearChild();
        }
        protected override void OnReset()
        {
            childID = null;
        }

        //        [SerializeField]
        //        private string text = "New Dialogue Text";
        //        public string Text
        //        {
        //            get => text;
        //#if UNITY_EDITOR
        //            set
        //            {
        //                if (text != value)
        //                {
        //                    Undo.RecordObject(this, "Update Node Text");
        //                    text = value;
        //                    EditorUtility.SetDirty(this);
        //                }
        //            }
        //#endif
        //        }

        //        [SerializeField]
        //        private Sprite portrait;
        //        public Sprite Portrait
        //        {
        //            get => portrait;
        //#if UNITY_EDITOR
        //            set
        //            {
        //                if (portrait != value)
        //                {
        //                    Undo.RecordObject(this, "Changed node portrait");
        //                    portrait = value;
        //                    EditorUtility.SetDirty(this);
        //                }
        //            }
        //#endif
        //        }

        //        [SerializeField]
        //        private string speaker;
        //        public string Speaker
        //        {
        //            get => speaker;
        //#if UNITY_EDITOR
        //            set
        //            {
        //                if (speaker != value)
        //                {
        //                    Undo.RecordObject(this, "Changed node speaker");
        //                    speaker = value;
        //                    EditorUtility.SetDirty(this);
        //                }
        //            }
        //#endif
        //        }
        //        [SerializeField]
        //        private DialogueOrientation orientation = DialogueOrientation.Left;
        //        public DialogueOrientation Orientation
        //        {
        //            get => orientation;
        //            set
        //            {
        //                if (orientation != value)
        //                {
        //                    Undo.RecordObject(this, "Changed Node Orientation");
        //                    orientation = value;
        //                    EditorUtility.SetDirty(this);
        //                }
        //            }
        //        }


        //#if UNITY_EDITOR
        //        [SerializeField]
        //        protected Rect rect = new Rect(10, 10, 300, 180);
        //        public override Rect NodeRect { get => rect; set => rect = value; }
        //        public override Rect GetBaseRect() => rect;
        //        public override void Reset()
        //        {
        //            Undo.RecordObject(this, "Reset DialogueNode");
        //            speaker = null;
        //            portrait = null;
        //            ChildID = null;
        //            text = "";
        //            orientation = DialogueOrientation.Left;
        //            EditorUtility.SetDirty(this);
        //        }
        //        public Vector2 boxScroll = Vector2.zero;
        //#endif
    }
}