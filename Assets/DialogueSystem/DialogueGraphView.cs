using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
//using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace DialogueSystem
{
	public class DialogueGraphView : GraphView, ISerializationCallbackReceiver
	{
		private readonly Vector2 defaultNodeSize = new Vector2(500, 200);
		private DialogueNode entryNode;
		public DialogueGraphView()
        {
			styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();
			//grid.
			AddElement(GenerateEntryPoint());
        }
		private DialogueNode CreateDialogueNode(string nodeName)
        {
			var node = new DialogueNode
			{
				title = nodeName,
				dialogueText = nodeName,
			};

			var inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
			inputPort.portName = "In";
			node.inputContainer.Add(inputPort);

			var button = new Button(() => { AddChoicePort(node); });
			button.text = "Add Choice";
			node.titleContainer.Add(button);

			node.RefreshExpandedState();
			node.RefreshPorts();
			node.SetPosition(new Rect(Vector2.zero, defaultNodeSize));

			return node;
        }
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
			var compatiblePorts = new List<Port>();
			ports.ForEach((port) =>
			{
				if (startPort != port && startPort.node != port.node)
					compatiblePorts.Add(port);
			});
			return compatiblePorts;
        }
        public void CreateNode(string nodeName)
        {
			AddElement(CreateDialogueNode(nodeName));
        }

		private Port GeneratePort(DialogueNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
        {
			return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float));
        }

		private DialogueNode GenerateEntryPoint()
        {
			var node = new DialogueNode
			{
				title = "Start",
				dialogueText = "Entry point",
				entryPoint = true
			};
			var newPort = GeneratePort(node, Direction.Output);
			newPort.portName = "Next";
			node.outputContainer.Add(newPort);

			node.RefreshExpandedState();
			node.RefreshPorts();
			node.SetPosition(new Rect(100, 100, 150, 100));

			return node;
        }
		private void AddChoicePort(DialogueNode node)
        {
			var newPort = GeneratePort(node, Direction.Output);
			int outputCount = node.outputContainer.Query("connector").ToList().Count;
			newPort.portName = $"Choise {outputCount}";
			node.outputContainer.Add(newPort);
			node.RefreshExpandedState();
			node.RefreshPorts();

        }
		public DialogueNode GetEntryNode() => entryNode;

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            
        }
    }	
}
