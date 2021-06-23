using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CoreTools;

namespace CoreTools.NodeSystem
{
    public interface ISingleChild
    {
        public string ChildID
        { 
            get;
            #if UNITY_EDITOR 
            set; 
            #endif 
        }

        public bool HasChild();
        public void ClearChild();
        public void ClearChild(string id);
    }
}