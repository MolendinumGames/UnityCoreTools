using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;
using CoreTools.NodeSystem;


namespace CoreTools.QuestSystem
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
    public class Quest : NodeHolder
    {
        protected override void SetupRootNode()
        {
            // Create QuestEntryNode that implements IMultiChild
            // Add entryNode to Quest
            // make entryNode == null check here
        }
    }
}