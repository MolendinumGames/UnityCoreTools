using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;

namespace CoreTools.Dialogue
{
    public class ChoiceNode : DialogueNode
    {
        public override string ChildID
        {
            get
            {
                Debug.Log("Cannot get childId from Choice node! Returning null");
                return null;
            }
            set => base.ChildID = value; // irrelevant
        }

        [SerializeField]
        List<ChoiceField> choices = new List<ChoiceField>();

        public int ChoiceAmount
        {
            get => choices.Count;
        }
        public List<string> GetAllChildren() => choices.Select(choice => choice.childId).ToList();

        public List<ChoiceField> GetAllChoices() => choices;

        public string[] GetAllChoiceTexts()
        {
            return choices.Select(choice => choice.text).ToArray();
        }
        public string GetChildOfChoice(int choiceId)
        {
            return choices[choiceId].childId;
        }
        public string GetTextOfChoice(int id)
        {
            return choices[id].text;
        }


#if UNITY_EDITOR
        public void RemoveChoice(int index)
        {
            Undo.RecordObject(this, "Removed Choice");
            choices.RemoveAt(index);
            EditorUtility.SetDirty(this);
        }
        public ChoiceField AddChoice()
        {
            Undo.RecordObject(this, "Added choice to node");
            ChoiceField newField = new ChoiceField();
            choices.Add(newField);
            EditorUtility.SetDirty(this);
            return newField;
        }
        public void SetChildOfChoice(int id, string childId)
        {
            if (choices[id].childId != childId)
            {
                Undo.RecordObject(this, "Changed Choices child of node");
                choices[id].childId = childId;
                EditorUtility.SetDirty(this);
            }
        }
        public void SetTextOfChoice(int id, string newText)
        {
            if (choices[id].text != newText)
            {
                Undo.RecordObject(this, "Changed Choice Text");
                choices[id].text = newText;
                EditorUtility.SetDirty(this);
            }
        }
        public void ClearIdFromChoices(string id)
        {
            Undo.RecordObject(this, "Removed child ID from all choices");
            foreach (ChoiceField field in GetAllChoices())
            {
                if (field.childId == id)
                    field.childId = null;
            }
            EditorUtility.SetDirty(this);
        }

    #region Layout Methods
        [SerializeField]
        Rect choiceRect = new Rect(10f, 10f, 300f, 200f);
        public override Rect NodeRect { get => GetChoiceNodeRect(); }
        public override void SetPosition(Vector2 newPos)
        {
            Undo.RecordObject(this, "Set Position of choice node");
            newPos.x = Mathf.Clamp(newPos.x, 0f, 4000f);
            newPos.y = Mathf.Clamp(newPos.y, 0f, 4000f);

            choiceRect = new Rect(newPos, choiceRect.size);
            EditorUtility.SetDirty(this);
        }
        public float GetChoiceHeight() => EditorGUIUtility.singleLineHeight * 1.5f + 2;
        protected Rect GetChoiceNodeRect()
        {
            float choiceHeight = GetChoiceHeight();
            float extra = choiceHeight * ChoiceAmount;
            Vector2 extraSize = new Vector2(0, extra);
            return new Rect(choiceRect.position, choiceRect.size + extraSize);
        }
        public Rect GetBasicRect()
        {
            return choiceRect;
        }
    #endregion
        public override void Reset()
        {
            // Undo records in base
            base.Reset();
            choices.Clear();
        }
#endif

    }
}