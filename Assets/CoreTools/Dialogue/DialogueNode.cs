using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace CoreTools.Dialogue
{
    public class DialogueNode : ScriptableObject
    {
        [SerializeField]
        public string uniqueID = "newId";

        [SerializeField]
        private string text;
        public string Text
        {
            get => text;
#if UNITY_EDITOR
            set
            {
                Undo.RecordObject(this, "Update Node Text");
                text = value;
                EditorUtility.SetDirty(this);
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
                Undo.RecordObject(this, "Changed node portrait");
                portrait = value;
                EditorUtility.SetDirty(this);
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
                Undo.RecordObject(this, "Changed node speaker");
                speaker = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

        [SerializeField]
        private string childId;
        public string ChildID
        {
            get => childId;
#if UNITY_EDITOR
            set
            {
                Undo.RecordObject(this, "Changed node childID");
                childId = value;
                EditorUtility.SetDirty(this);
            }
#endif
        }

#if UNITY_EDITOR
        [SerializeField]
        public Rect rect = new Rect(10, 10, 360, 245);
        [SerializeField]
        public Vector2 scroll = Vector2.zero;
#endif
        public bool HasChild() => !string.IsNullOrEmpty(ChildID);
        public void ResetValues()
        {
            speaker = null;
            portrait = null;
            childId = null;
            text = "";
        }
    }
}