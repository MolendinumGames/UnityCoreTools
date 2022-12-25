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
using CoreTools.NodeSystem;
using CoreTools.DialogueSystem;

public abstract class GraphDrawer
{
    protected NodeEditorWindow nodeEditor;
    protected float oldLabelWidth;
    public GraphDrawer(NodeEditorWindow nodeEditor)
    {
        this.nodeEditor = nodeEditor;
        oldLabelWidth = EditorGUIUtility.labelWidth;
        GenerateStyles();
    }

    protected GUIStyle nodeStyle;
    protected GUIStyle selectedStyle;
    protected GUIStyle entryStyle;

    public readonly float tangentOffset = 100f;
    public readonly float connectionWidth = 4f;
    protected readonly Vector2 radioButtonSize = new Vector2(15f, 15f);

    public abstract void DrawGraphNode(GraphNode node);

    protected void GenerateStyles()
    {
        // Standard Node Style
        nodeStyle = new GUIStyle()
        {
            padding = new RectOffset(20, 20, 15, 15),
            border = new RectOffset(33, 33, 33, 33),
        };
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        nodeStyle.normal.textColor = Color.white;

        // Entry Node Style
        entryStyle = new GUIStyle()
        {
            border = new RectOffset(22, 22, 22, 22),
            padding = new RectOffset(20, 20, 15, 15),
            alignment = TextAnchor.MiddleCenter,
        };
        entryStyle.normal.textColor = Color.white;
        entryStyle.normal.background = EditorGUIUtility.Load("node3") as Texture2D;
        entryStyle.fontSize *= 2;

        // Selected Node Style
        selectedStyle = new GUIStyle(nodeStyle);
        selectedStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;

        // setting header style here based on EditorStyles.boldLabel will cause NullRef
        // create new headerstyle inside DrawHeader instead
    }

    protected void DrawHeader(GraphNode node)
    {
        var headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.UpperLeft
        };
        string headerName = node.GetType().GetLastTypeAsString();

        GUILayout.BeginHorizontal();

        GUILayout.Label(headerName, headerStyle);
        if (GUILayout.Button("Delete", GUILayout.Width(80f)))
        {
            nodeEditor.MarkNodeToRemove(node);
        }

        GUILayout.EndHorizontal();
    }
    protected void DrawInConnector(GraphNode node)
    {
        Vector2 pos = GetSingleInConnectorPos(node);
        Rect buttonRect = new Rect(pos, radioButtonSize);
        GUIStyle style = new GUIStyle(EditorStyles.radioButton);
        if (nodeEditor.selectedGraph.NodeHasParent(node))
        {
            style.normal = style.onActive;
        }

        if (GUI.Button(buttonRect, GUIContent.none, style))
        {
            if (nodeEditor.findingChildNode != null &&
                nodeEditor.findingChildNode != node)
            {
                nodeEditor.SetChildOfFindingChildNode(node);
            }
            else
            {
                nodeEditor.SetFindingParentNode(node);
            }
        }
    }
    protected void DrawOutConnector(GraphNode node)
    {
        Vector2 pos = GetSingleOutConnectorPos(node);
        Rect buttonRect = new Rect(pos, radioButtonSize);
        GUIStyle style = new GUIStyle(EditorStyles.radioButton);
        if (nodeEditor.selectedGraph.NodeHasChild(node))
        {
            style.normal = style.onActive;
        }

        if (GUI.Button(buttonRect, GUIContent.none, style))
        {
            if (nodeEditor.findingParentNode != null &&
                nodeEditor.findingParentNode != node)
            {
                nodeEditor.SetParentOfFindingParentNode(node, -1);
            }
            else
            {
                nodeEditor.SetFindingChildNode(node);
            }
        }
    }
    protected void DrawChoiceOutConnectors(GraphNode node)
    {
        IChoiceContainer choiceParent = (IChoiceContainer)node;
        for (int i = 0; i < choiceParent.ChoiceAmount; i++)
        {
            Vector2 pos = GetChoiceOutConnectorPos(node, i);
            Rect buttonRect = new Rect(pos, radioButtonSize);

            bool hasChild = !string.IsNullOrEmpty(choiceParent.GetChildOfChoice(i));
            GUIStyle style = new GUIStyle(EditorStyles.radioButton);
            if (hasChild)
                style.normal = style.onActive;

            if (GUI.Button(buttonRect, GUIContent.none, style))
            {
                if (nodeEditor.findingParentNode != null &&
                    nodeEditor.findingParentNode != node)
                {
                    nodeEditor.SetParentOfFindingParentNode(node, i);
                }
                else
                {
                    nodeEditor.SetFindingChildNode(node, i);
                }
            }
        }
    }

    public Vector2 GetSingleInConnectorPos(GraphNode node)
    {
        var nodePos = node.NodeRect.position;
        var xOffset = 0f;
        var yOffset = node.GetBaseRect().height * 0.5f - 5f;
        Vector2 target = nodePos + new Vector2(xOffset, yOffset);
        return target;
    }
    public Vector2 GetSingleOutConnectorPos(GraphNode node)
    {
        Vector2 nodePos = node.NodeRect.position;
        float xOffset = node.GetBaseRect().width - 15f;
        float yOffset = node.GetBaseRect().height * .5f - 5f;
        Vector2 target = nodePos + new Vector2(xOffset, yOffset);
        return target;
    }
    public Vector2 GetChoiceOutConnectorPos(GraphNode node, int choiceId)
    {
        IChoiceContainer choiceParent = (IChoiceContainer)node;

        Vector2 nodePos = node.NodeRect.position;
        float xOffset = node.GetBaseRect().width - 15f;
        float yOffset = node.GetBaseRect().height - 16f;
        Vector2 choiceAreaPos = nodePos + new Vector2(xOffset, yOffset);

        float choiceHeight = choiceParent.GetChoiceHeight();
        float areaOffset = choiceId * choiceHeight - (0.5f * choiceHeight);
        Vector2 pos = choiceAreaPos + new Vector2(0, areaOffset);
        return pos;
    }
    public void DrawParentSearchConnection(GraphNode searchNode, Vector2 mousePos)
    {
        Vector3 offsetVector = GetRadioCenterOffset();
        Vector3 inConnectPos = GetSingleInConnectorPos(searchNode);
        Vector3 startPos = inConnectPos + offsetVector;

        Vector3 endPos = mousePos;

        Vector3 startTangent = startPos + (Vector3.left * tangentOffset);
        Vector3 endTangent = endPos + (Vector3.right * tangentOffset);

        Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.red, null, connectionWidth);
    }
    public void DrawChildSearchConnection(GraphNode searchNode, Vector2 mousePos)
    {
        Vector3 offsetVector = GetRadioCenterOffset();
        Vector3 outConnectorPos = GetSingleOutConnectorPos(searchNode);
        Vector3 startPos = outConnectorPos + offsetVector;

        Vector3 endPos = mousePos;

        Vector3 startTangent = startPos + (Vector3.right * tangentOffset);
        Vector3 endTangent = endPos + (Vector3.left * tangentOffset);

        Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.red, null, connectionWidth);
    }
    public void DrawChildSearchConnection(GraphNode searchNode, int choiceId, Vector2 mousePos)
    {
        Vector3 offsetVector = GetRadioCenterOffset();
        Vector3 outConnectorPos = GetChoiceOutConnectorPos(searchNode, choiceId);
        Vector3 startPos = outConnectorPos + offsetVector;

        Vector3 endPos = mousePos;

        Vector3 startTangent = startPos + (Vector3.right * tangentOffset);
        Vector3 endTangent = endPos + (Vector3.left * tangentOffset);

        Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.red, null, connectionWidth);
    }
    public void DrawConnections(GraphNode parent)
    {
        if (parent is IChoiceContainer choiceParent)
        {
            for (int i = 0; i < choiceParent.ChoiceAmount; i++)
            {
                string childId = choiceParent.GetChildOfChoice(i);

                if (string.IsNullOrEmpty(childId))
                    continue;

                var childNode = nodeEditor.selectedGraph.GetAnyGraphNode(childId);
                if (childNode == null)
                    continue;

                Vector3 offset = GetRadioCenterOffset();
                Vector3 startPos = (Vector3)GetChoiceOutConnectorPos(parent, i) + offset;
                Vector3 startTangent = startPos + Vector3.right * tangentOffset;
                Vector3 endPos = (Vector3)GetSingleInConnectorPos(childNode) + offset;
                Vector3 endTangent = endPos + Vector3.left * tangentOffset;
                Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.white, null, connectionWidth);
            }
        }
        else
        {
            Vector3 offset = GetRadioCenterOffset();
            List<Vector3> targets = new List<Vector3>();
            if (parent is ISingleChild singleParent
                && singleParent.HasChild())
            {
                string childId = singleParent.ChildID;
                var childNode = nodeEditor.selectedGraph.GetAnyGraphNode(childId);
                if (childNode != null)
                {
                    targets.Add((Vector3)GetSingleInConnectorPos(childNode) + offset);
                }
            }
             if (parent is IMultiChild multiParent)
             {
                foreach (var childId in multiParent.GetChildren())
                {
                    if (!string.IsNullOrEmpty(childId))
                    {
                        var targetNode = nodeEditor.selectedGraph.GetAnyGraphNode(childId);
                        if (targetNode == null)
                            continue;
                        Vector3 newTarget = (Vector3)GetSingleInConnectorPos(targetNode) + offset;
                        targets.Add(newTarget);
                    }
                }
             }
            //Draw
            Vector3 startPos = (Vector3)GetSingleOutConnectorPos(parent) + offset;
            Vector3 startTangent = startPos + Vector3.right * tangentOffset;
            for (int i = 0; i < targets.Count; i++)
            {
                Vector3 endTangent = targets[i] + Vector3.left * tangentOffset;
                Handles.DrawBezier(startPos, targets[i], startTangent, endTangent, Color.white, null, connectionWidth);
            }
        }
    }
    public Vector3 GetRadioCenterOffset()
    {
        float offsetValue = radioButtonSize.x * .5f;
        return new Vector3(offsetValue, offsetValue, 0);
    }
    protected virtual void DrawFloatEventNode(FloatEventNode node)
    {
        GUIStyle useStyle = nodeEditor.focusedNode == node ? selectedStyle : nodeStyle;
        GUILayout.BeginArea(node.NodeRect, useStyle);

        DrawHeader(node);
        EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
        node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(FloatChannel), false) as FloatChannel;
        node.Value = EditorGUILayout.FloatField("Value: ", node.Value);
        EditorGUIUtility.labelWidth = oldLabelWidth;

        GUILayout.EndArea();

        DrawInConnector(node);
        DrawOutConnector(node);
    }

    protected virtual void DrawStringEventNode(StringEventNode node)
    {
        GUIStyle useStyle = nodeEditor.focusedNode == node ? selectedStyle : nodeStyle;
        GUILayout.BeginArea(node.NodeRect, useStyle);

        DrawHeader(node);
        EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
        node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(StringChannel), false) as StringChannel;
        node.Value = EditorGUILayout.TextField("Value: ", node.Value);
        EditorGUIUtility.labelWidth = oldLabelWidth;

        GUILayout.EndArea();

        DrawInConnector(node);
        DrawOutConnector(node);
    }

    protected virtual void DrawIntEventNode(IntEventNode node)
    {
        GUIStyle useStyle = nodeEditor.focusedNode == node ? selectedStyle : nodeStyle;
        GUILayout.BeginArea(node.NodeRect, useStyle);

        DrawHeader(node);

        EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
        node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(IntChannel), false) as IntChannel;
        node.Value = EditorGUILayout.IntField("Value: ", node.Value);
        EditorGUIUtility.labelWidth = oldLabelWidth;

        GUILayout.EndArea();

        DrawInConnector(node);
        DrawOutConnector(node);
    }

    protected virtual void DrawBoolEventNode(BoolEventNode node)
    {
        GUIStyle useStyle = nodeEditor.focusedNode == node ? selectedStyle : nodeStyle;
        GUILayout.BeginArea(node.NodeRect, useStyle);

        DrawHeader(node);

        EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
        node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(BoolChannel), false) as BoolChannel;
        node.Value = EditorGUILayout.Toggle("Value: ", node.Value);
        EditorGUIUtility.labelWidth = oldLabelWidth;

        GUILayout.EndArea();

        DrawInConnector(node);
        DrawOutConnector(node);
    }

    protected virtual void DrawVoidEventNode(VoidEventNode node)
    {
        GUIStyle useStyle = nodeEditor.focusedNode == node ? selectedStyle : nodeStyle;
        GUILayout.BeginArea(node.NodeRect, useStyle);

        DrawHeader(node);

        EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
        node.Channel = EditorGUILayout.ObjectField("Channel: ", node.Channel, typeof(VoidChannel), false) as VoidChannel;

        GUILayout.EndArea();

        DrawInConnector(node);
        DrawOutConnector(node);
    }
    protected virtual void DrawDialogueEventNode(DialogueEventNode node)
    {
        GUIStyle useStyle = nodeEditor.focusedNode == node ? selectedStyle : nodeStyle;
        GUILayout.BeginArea(node.NodeRect, useStyle);

        DrawHeader(node);

        EditorGUIUtility.labelWidth = Mathf.FloorToInt(oldLabelWidth * .4f);
        node.Channel = EditorGUILayout.ObjectField("Channel: ",
                                                   node.Channel,
                                                   typeof(DialogueChannel),
                                                   false) as DialogueChannel;
        GUILayout.EndArea();
        DrawInConnector(node);
        DrawOutConnector(node);
    }
}
