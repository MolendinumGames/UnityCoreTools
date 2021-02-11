using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
	public class DialogueNode : Node
	{
		private string uniqueID;

		public string dialogueText;
		public bool entryPoint = false;

		public string GetUniqueID()
        {
			if (string.IsNullOrWhiteSpace(uniqueID))
				uniqueID = System.Guid.NewGuid().ToString();
			return uniqueID;
        }

		public DialogueNode()
        {
			uniqueID = System.Guid.NewGuid().ToString();
		}
	}	
}
