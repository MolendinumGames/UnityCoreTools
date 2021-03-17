using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.NodeSystem;

public abstract class NodeDrawer
{
    protected NodeEditorWindow nodeEditor;
    public NodeDrawer(NodeEditorWindow nodeEditor)
    {
        this.nodeEditor = nodeEditor;
    }
    public readonly float tangentOffset = 100f;
    public readonly float connectionWidth = 4f;
    protected readonly Vector2 radioButtonSize = new Vector2(15f, 15f);
    public abstract void DrawGraphNode(GraphNode node);
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
    public Vector3 GetRadioCenterOffset()
    {
        float offsetValue = radioButtonSize.x * .5f;
        return new Vector3(offsetValue, offsetValue, 0);
    }
}
