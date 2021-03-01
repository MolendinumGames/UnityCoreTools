using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CoreTools.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private static readonly string windowTitle = "Dialogue Window";

        private NodeDrawer nodeDrawer;

        public DialogueSO selectedDialogue;

        private DialogueNode draggedNode = null;
        private Vector2 dragOffset = Vector2.zero;

        public DialogueNode findingParentNode = null;
        public DialogueNode findingChildNode = null;

        //public readonly Vector2 radioButtonSize = new Vector2(10f, 10f);
        //public GUIStyle radioActiveStyle;
        private Vector2 lastMousePos = Vector2.zero;

        private DialogueNode creatingNode;
        private DialogueNode removeNode;

        [MenuItem("Tools/DialogueEditor")]
        public static void OpenWindow()
        {
            var window = GetWindow<DialogueEditor>(windowTitle);
            window.Show();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            DialogueSO targetObj = EditorUtility.InstanceIDToObject(instanceID) as DialogueSO;
            if (targetObj != null)
            {
                OpenWindow();
                return true;
            }
            return false;
        }
        private void OnEnable()
        {
            nodeDrawer = new NodeDrawer(this);
            ClearConnectingNodes();
        }
        private void OnSelectionChange()
        {
            if (Selection.activeObject is DialogueSO)
            {
                selectedDialogue = Selection.activeObject as DialogueSO;
                GUI.changed = true;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No Dialogue selected!");
                return;
            }
            //DrawToolBar();
            DrawGraph();
            ProcessEvents();

            if (creatingNode != null)
                CreateNewChildNode();
            if (removeNode != null)
                RemoveNode(removeNode);
        }
        private void DrawGraph()
        {
            foreach (DialogueNode node in selectedDialogue.GetNodes())
            {
                nodeDrawer.DrawNode(node);
                DrawConnection(node);
            }
            DrawSearchingNodeConnection();
        }
        private void DrawSearchingNodeConnection()
        {
            if (findingParentNode != null)
            {
                float offsetValue = nodeDrawer.radioButtonSize.x * .5f;
                Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);
                Vector3 startPoint = (Vector3)nodeDrawer.GetInConnectorPos(findingParentNode) + offsetVector;
                Vector3 endPoint = Event.current.mousePosition;
                Vector3 startTangent = startPoint + (Vector3.left * 50f);
                Vector3 endTangent = endPoint + (Vector3.right * 50f);

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.red, null, 2f);
                Repaint();
            }
            else if (findingChildNode != null)
            {
                float offsetValue = nodeDrawer.radioButtonSize.x * .5f;
                Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);
                Vector3 startPoint = (Vector3)nodeDrawer.GetOutConnectorPos(findingChildNode) + offsetVector;
                Vector3 endPoint = Event.current.mousePosition;
                Vector3 startTangent = startPoint + (Vector3.right * 50f);
                Vector3 endTangent = endPoint + (Vector3.left * 50f);

                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.red, null, 2f);
                Repaint();
            }
        }

        private void DrawConnection(DialogueNode node)
        {
            DialogueNode childNode = selectedDialogue.GetChildOfNode(node);
            if (childNode != null)
            {
                float offsetValue = nodeDrawer.radioButtonSize.x * .5f;
                Vector3 offsetVector = new Vector3(offsetValue, offsetValue, 0);
                Vector3 startPoint = (Vector3)nodeDrawer.GetOutConnectorPos(node) + offsetVector;
                Vector3 endPoint = (Vector3)nodeDrawer.GetInConnectorPos(childNode) + offsetVector;
                Vector3 startTangent = startPoint + (Vector3.right * 50f);
                Vector3 endTangent = endPoint + (Vector3.left * 50f);
                Handles.DrawBezier(startPoint, endPoint, startTangent, endTangent, Color.white, null, 2f);
            }
        }
        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0)
                {
                    OnLeftClick();
                }
                else if (Event.current.button == 1)
                {
                    OnRightClick();
                }
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                OnMouseDrag();
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                OnMouseUp();
            }
        }
        private DialogueNode GetNodeAtPoint(Vector2 mousePos)
        {
            // returns he LAST node from GetNodes() that matches the position
            DialogueNode targetNode = null;
            foreach (DialogueNode node in selectedDialogue.GetNodes())
            {
                if (node.rect.Contains(mousePos))
                {
                    targetNode = node;
                }
            }
            return targetNode;
        }
        public void ClearConnectingNodes()
        {
            findingChildNode = null;
            findingParentNode = null;
        }

        private void OnLeftClick()
        {
            draggedNode = GetNodeAtPoint(Event.current.mousePosition);
            if (draggedNode != null)
            {
                dragOffset = draggedNode.rect.position - Event.current.mousePosition;
            }
            if (findingChildNode != null)
                findingChildNode.ChildID = null;
            if (findingParentNode != null)
            {
                foreach (DialogueNode parent in selectedDialogue.GetParentNodes(findingParentNode))
                {
                    parent.ChildID = null;
                }
            }

            ClearConnectingNodes();
        }
        private void OnRightClick()
        {
            ClearConnectingNodes();
        }
        private void OnMouseUp()
        {
            draggedNode = null;
            dragOffset = Vector2.zero;
        }
        private void OnMouseDrag()
        {
            if (draggedNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Moved Dialogue Node");
                draggedNode.rect.position = Event.current.mousePosition + dragOffset;
                GUI.changed = true;
            }
        }
        private void DrawToolBar()
        {
            string[] tools = { "tool1", "tool2", "tool3", "tool4" };
            int newSelection = GUILayout.Toolbar(-1, tools);
        }
        public void MarkAsCreationNode(DialogueNode node)
        {
            creatingNode = node;
        }
        private void CreateNewChildNode()
        {
            DialogueNode newNode = selectedDialogue.CreateNode(creatingNode);
            float xOffset = creatingNode.rect.width + 50f;
            float yOffset = 20f;
            Vector2 newPosition = creatingNode.rect.position + new Vector2(xOffset, yOffset);
            newNode.rect.position = newPosition;
            creatingNode = null;
            Repaint();
        }
        public void MarkNodeToRemove(DialogueNode node)
        {
            removeNode = node;
        }
        private void RemoveNode(DialogueNode node)
        {
            selectedDialogue.RemoveNode(node);
            removeNode = null;
            Repaint();
        }
    }
}