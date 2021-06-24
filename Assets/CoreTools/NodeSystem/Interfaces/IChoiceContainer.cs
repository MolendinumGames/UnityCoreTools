using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools.NodeSystem
{
    public interface IChoiceContainer
    {
        public void SetChildOfChoice(int id, string child);
        public string GetChildOfChoice(int id);
        public void ClearChild(string child);
        public List<string> GetAllChildren();
        public bool HasChild();
        public bool HasChild(string child);
        
        public abstract int ChoiceAmount { get; }

#if UNITY_EDITOR
        public float GetChoiceHeight();
#endif

    }
}