using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
	[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
	public class DialogueObject : ScriptableObject
	{
		private DialogueGraphView dialogueGraph;

		public DialogueGraphView GetGraph()
        {
			if (dialogueGraph == null)
            {
				ConstructNewGraph();
            }
			return dialogueGraph;
        }

		private void ConstructNewGraph()
        {
            dialogueGraph = new DialogueGraphView
            {
                name = "this.name"
            };
        }
	}	
}
