/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.Pooling
{
    [System.Serializable]
    public class LinkedPoolObjectList :  IEnumerable<LinkedPoolObjectNode>
    {
        public LinkedPoolObjectNode First { get; set; } = null;

        public LinkedPoolObjectNode Last { get; set; } = null;

        public bool IsEmpty { get => First == null; }

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

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<LinkedPoolObjectNode>)this).GetEnumerator();
        }

        IEnumerator<LinkedPoolObjectNode> IEnumerable<LinkedPoolObjectNode>.GetEnumerator()
        {
            foreach (var node in GetNodes())
                yield return node;
        }

        public IEnumerable<LinkedPoolObjectNode> GetNodes()
        {
            var current = First;
            do
            {
                yield return current;
                current = current.Next;
            }
            while (current != null);
        }

        public IEnumerable<GameObject> GetGameObjects()
        {
            var current = First;
            do
            {
                if (current.PooledObject == null)
                    throw new NullReferenceException("LinkedPoolObjectNode doesn't have a valid GameObject reference.");

                yield return current.PooledObject;
                current = current.Next;
            }
            while (current != null);
        }

        /// <summary>
        /// Add a new node to the beginning.
        /// </summary>
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

        /// <summary>
        /// Add a new node for the GameObject to the beginning.
        /// </summary>
        public void PrependNew(GameObject go)
        {
            Prepend(new LinkedPoolObjectNode(go));
        }


        /// <summary>
        /// Add a node to the end.
        /// </summary>
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

        /// <summary>
        /// Add a new node for the GameObject to the end.
        /// </summary>
        public void AppendNew(GameObject go)
        {
            Append(new LinkedPoolObjectNode(go));
        }

        /// <summary>
        /// Clears the list but will not destroy the by the nodes referenced GameObjects.
        /// </summary>
        public void Clear()
        {
            First = null;
            Last = null;
        }

        /// <summary>
        /// Remove the node at index and returns its data.
        /// </summary>
        /// <returns>The referenced pooled GameObject of the removed node.</returns>
        public GameObject RemoveAt(int index)
        {
            if (index < 0 ||
                IsEmpty ||
                index >= Count)
            {
                throw new IndexOutOfRangeException($"LinkedPoolList doesn't have an index: {index}");
            }

            LinkedPoolObjectNode current = First;

            // Get the node to remove
            for (int i = 0; i < index; i++)
                current = current.Next;

            // Get back the GameObject of the removed node
            GameObject orphantGameObject = Remove(current);
            return orphantGameObject;
        }

        /// <summary>
        /// Remove the given node from this linked list.
        /// </summary>
        /// <returns>The pooled GameObject from the removed node.</returns>
        public GameObject Remove(LinkedPoolObjectNode node)
        {
            bool hasPrevious = node.Previous != null;
            bool hasNext = node.Next != null;

            // Change First node
            if (node == First)
                First = hasNext ? node.Next : null;

            // Change Last Node
            if (node == Last)
                Last = hasPrevious ? node.Previous : null;

            // Relink the previous node
            if (hasPrevious)
                node.Previous = hasNext ? node.Next : null;

            // Relink the next node
            if (hasNext)
                node.Next = hasPrevious ? node.Previous : null;

            return node.PooledObject;
        }

        public void MoveToEnd(LinkedPoolObjectNode node)
        {
            Remove(node);

            Append(node);
        }

        public void MoveToStart(LinkedPoolObjectNode node)
        {
            Remove(node);

            Prepend(node);
        }

        private void SetInitialNode(LinkedPoolObjectNode node)
        {
            First = node;
            Last = node;
        }
    }
}
