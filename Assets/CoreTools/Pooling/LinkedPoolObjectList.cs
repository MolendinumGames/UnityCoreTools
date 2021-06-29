using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CoreTools.Pooling
{
    class LinkedPoolObjectList
    {
        // First
        LinkedPoolObjectNode First { get; set; } = null;

        // Last
        LinkedPoolObjectNode Last { get; set; } = null;

        // Empty
        public bool IsEmpty { get => First == null; }

        // Count
        public int Count
        {
            get
            {
                if (IsEmpty)
                    return 0;

                int counter = 1;
                var current = First;
                while (current.Next != null)
                {
                    counter++;
                    current = current.Next;
                }
                return counter;
            }
        }

        // Prepend
        public void Prepend(LinkedPoolObjectNode node)
        {
            if (IsEmpty)
            {
                SetInitialNode(node);
            }
            else
            {
                node.Next = First;
                node.Previous = null;
                First.Previous = node;
                First = node;
            }
        }

        public void PrependNew(GameObject go)
        {
            Prepend(new LinkedPoolObjectNode(go));
        }

        // Append
        public void Append(LinkedPoolObjectNode node)
        {
            if (IsEmpty)
            {
                SetInitialNode(node);
            }
            else
            {
                node.Previous = Last;
                node.Next = null;
                Last.Next = node;
                Last = node;
            }
        }

        public void AppendNew(GameObject go)
        {
            Append(new LinkedPoolObjectNode(go));
        }

        // Clear
        public void Clear()
        {
            First = null;
            Last = null;
        }

        private void SetInitialNode(LinkedPoolObjectNode node)
        {
            First = node;
            Last = node;
        }


    }
}
