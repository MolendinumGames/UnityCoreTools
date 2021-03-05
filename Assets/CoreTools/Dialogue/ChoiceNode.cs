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
        [SerializeField]
        int selectedPath = 0;
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
        public List<ChoiceField> GetAllChoices() => choices;

        public string GetChildOfChoice(int choiceId)
        {
            return choices[choiceId].childId;
        }
        public void RemoveChoice(int index)
        {
            choices.RemoveAt(index);
        }
        public ChoiceField AddChoice()
        {
            ChoiceField newField = new ChoiceField();
            choices.Add(newField);
            return newField;
        }
        public string[] GetAllChoiceTexts()
        {
            return choices.Select(choice => choice.text).ToArray();
        }
        public List<string> GetAllChildren() => choices.Select(choice => choice.childId).ToList();
        public void ClearIdFromChoices(string id)
        {
            foreach (ChoiceField field in GetAllChoices())
            {
                if (field.childId == id)
                    field.childId = null;
            }
        }
        public override void SetPosition(Vector2 newPos)
        {
            newPos.x = Mathf.Clamp(newPos.x, 0f, 4000f);
            newPos.y = Mathf.Clamp(newPos.y, 0f, 4000f);

            choiceRect = new Rect(newPos, choiceRect.size);
        }
        public float GetChoiceHeight() => EditorGUIUtility.singleLineHeight * 1.5f + 2;
        protected Rect GetChoiceNodeRect()
        {
            float choiceHeight = GetChoiceHeight();
            float extra = choiceHeight * ChoiceAmount;
            Vector2 extraSize = new Vector2(0, extra);
            return new Rect(choiceRect.position, choiceRect.size + extraSize);
        }
        [SerializeField]
        Rect choiceRect = new Rect(10f, 10f, 300f, 200f);
        public override Rect NodeRect { get => GetChoiceNodeRect(); }
        public Rect GetBasicRect()
        {
            return choiceRect;
        }
        public override void Reset()
        {
            // Undo records in base
            base.Reset();
            choices.Clear();
        }

    }
}