using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;
using CoreTools.NodeSystem;

public class NodeDrawer : MonoBehaviour
{
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
}
