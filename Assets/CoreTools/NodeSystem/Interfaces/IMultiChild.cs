using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.NodeSystem
{
    public interface IMultiChild
    {
        public IEnumerable<string> GetChildren();

        public bool HasChild(string id);
        public void ClearChild(string id);
        public void AddChild(string id);

        public int ChildAmount { get; }
    }
}