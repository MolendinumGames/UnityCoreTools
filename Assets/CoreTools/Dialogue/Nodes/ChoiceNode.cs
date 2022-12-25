/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem
{
    public class ChoiceNode : DialogueNode, IChoiceContainer
    {
        public virtual bool HasChild()
        {
            return choices.Where(x => !string.IsNullOrEmpty(x.ChildID)).ToArray().Length > 0;
        }
        public virtual bool HasChild(string childId)
        {
            return choices.Where(x => x.ChildID == childId).ToArray().Length > 0;
        }
        public void ClearAllChildren()
        {
            Undo.RecordObject(this, "Cleared all choices form ChoiceNode");
            choices.Clear();
        }
        public void ClearChild(string id)
        {
            ClearIdFromChoices(id);
        }

        [SerializeField]
        List<ChoiceField> choices = new List<ChoiceField>();

        public int ChoiceAmount
        {
            get => choices.Count;
        }
        public List<string> GetAllChildren() => choices.Select(choice => ((ISingleChild)choice).ChildID).ToList();

        public List<ChoiceField> GetAllChoices() => choices;

        public string[] GetAllChoiceTexts()
        {
            return choices.Select(choice => choice.text).ToArray();
        }
        public string GetChildOfChoice(int choiceId)
        {
            return ((ISingleChild)choices[choiceId]).ChildID;
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
            ISingleChild choiceAsParent = choices[id];
            if (choiceAsParent.ChildID != childId)
            {
                Undo.RecordObject(this, "Changed Choices child of node");
                choiceAsParent.ChildID = childId;
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
                if (field.ChildID == id)
                    field.ChildID = null;
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
        public override Rect GetBaseRect()
        {
            return choiceRect;
        }
        protected override void OnReset()
        {
            // Undo records in Reset of base
            choices.Clear();
        }
    #endregion
#endif
    }
}