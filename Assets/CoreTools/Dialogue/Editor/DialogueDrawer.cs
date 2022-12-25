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
using CoreTools;
using CoreTools.DialogueSystem;
using CoreTools.NodeSystem;

namespace CoreTools.DialogueSystem.Editor
{
    public class DialogueDrawer : GraphDrawer
    {
        protected readonly DialogueEditorWindow dialogueEditor;

        public GUIStyle radioActiveStyle;
        public DialogueDrawer(DialogueEditorWindow dialogueEditor, NodeEditorWindow nodeEditor) 
            : base(nodeEditor)
        {
            this.nodeEditor = nodeEditor;
            this.dialogueEditor = dialogueEditor;
        }

        public override void DrawGraphNode(GraphNode node)
        {
            switch (node)
            {
                case DialogueEntryNode n:
                    DrawEntryNode(n);
                    break;
                case ChoiceNode n:
                    DrawChoiceNode(n);
                    break;
                case TextNode n:
                    DrawStandardNode(n);
                    break;
                case VoidEventNode n:
                    DrawVoidEventNode(n);
                    break;
                case BoolEventNode n:
                    DrawBoolEventNode(n);
                    break;
                case IntEventNode n:
                    DrawIntEventNode(n);
                    break;
                case FloatEventNode n:
                    DrawFloatEventNode(n);
                    break;
                case StringEventNode n:
                    DrawStringEventNode(n);
                    break;
                case DialogueEventNode n:
                    DrawDialogueEventNode(n);
                    break;
            }
            DrawConnections(node);
        }

        private void DrawEntryNode(DialogueEntryNode node)
        {
            GUILayout.BeginArea(node.NodeRect, entryStyle);
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = Mathf.CeilToInt(headerStyle.fontSize * 1.5f);
            EditorGUILayout.LabelField("Entry", headerStyle, GUILayout.Height(headerStyle.lineHeight * 1.8f));
            GUILayout.EndArea();
            DrawOutConnector(node);
        }
        private void DrawChoiceNode(ChoiceNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);
            DrawHeader(node);
            GUILayout.Space(1f);

            GUILayout.BeginHorizontal();
            node.Portrait = DrawIconField(node);

            GUILayout.BeginVertical();
            node.Speaker = DrawSpeakerField(node);
            node.Orientation = (DialogueOrientation)EditorGUILayout.EnumPopup(node.Orientation);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();

            GUILayout.Space(1f);

            node.Text = DrawDialogueBox(node);

            int toRemoveChoice = -1;
            for (int i = 0; i < node.ChoiceAmount; i++)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox,
                                          GUILayout.Height(EditorGUIUtility.singleLineHeight),
                                          GUILayout.Width(node.NodeRect.width - 32f));

                if (GUILayout.Button("X", GUILayout.Width(EditorGUIUtility.singleLineHeight)))
                {
                    toRemoveChoice = i;
                }

                GUILayout.Label(i.ToString(), GUILayout.Width(18f));
                string newText = EditorGUILayout.TextField(GUIContent.none, node.GetTextOfChoice(i));
                node.SetTextOfChoice(i, newText);

                GUILayout.EndHorizontal();
                GUILayout.Space(2f);
            }

            if (toRemoveChoice >= 0)
            {
                node.RemoveChoice(toRemoveChoice);
                dialogueEditor.Repaint();
            }

            // Draw Add Choice Button
            if (GUILayout.Button("Add Choice"))
            {
                node.AddChoice();
                dialogueEditor.Repaint();
            }

            GUILayout.EndArea();

            DrawInConnector(node);
            DrawChoiceOutConnectors(node);
        }
        private void DrawStandardNode(TextNode node)
        {
            GUIStyle useStyle = dialogueEditor.focusedNode == node ? selectedStyle : nodeStyle;
            GUILayout.BeginArea(node.NodeRect, useStyle);
            EditorGUIUtility.labelWidth = oldLabelWidth * .5f;

            DrawHeader(node);
            GUILayout.Space(1f);

            GUILayout.BeginHorizontal();
            node.Portrait = DrawIconField(node);

            GUILayout.BeginVertical();
            node.Speaker = DrawSpeakerField(node);
            node.Orientation = (DialogueOrientation)EditorGUILayout.EnumPopup(node.Orientation);
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(1f);

            node.Text = DrawDialogueBox(node);

            GUILayout.EndArea();

            EditorGUIUtility.labelWidth = oldLabelWidth;

            DrawInConnector(node);
            DrawOutConnector(node);
        }

        private Sprite DrawIconField(DialogueNode node)
        {
            EditorGUIUtility.labelWidth = .001f;
            var newSprite = (Sprite)EditorGUILayout.ObjectField(GUIContent.none, node.Portrait, typeof(Sprite), allowSceneObjects: false);
            EditorGUIUtility.labelWidth = oldLabelWidth * .5f;
            return newSprite;
        }
        private string DrawSpeakerField(DialogueNode node)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Speaker: ");
            string newSpeaker = EditorGUILayout.TextField(GUIContent.none, node.Speaker);
            EditorGUILayout.EndVertical();
            return newSpeaker;
        }
        private string DrawDialogueBox(DialogueNode node)
        {
            GUIStyle newStyle = new GUIStyle
            {
                fixedHeight = EditorGUIUtility.singleLineHeight * 3 + 5f
            };

            GUILayout.BeginVertical(newStyle);
            node.boxScroll = GUILayout.BeginScrollView(node.boxScroll, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUIStyle.none);

            EditorStyles.textField.wordWrap = true;
            string newText = GUILayout.TextArea(node.Text, GUILayout.ExpandHeight(true));

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            return newText;
        }
    }
}