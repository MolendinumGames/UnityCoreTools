using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoreTools;

namespace SaveSystem
{
    public class SaveableEntity : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        [SerializeField] private string uniqueId = "";
        public string GetUniqueID() => uniqueId;

        public object CaptureStates()
        {
            Dictionary<string, object> stateBlock = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                stateBlock[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return stateBlock;
        }
        public void RestoreStates(object stateData)
        {
            Dictionary<string, object> stateBlock = (Dictionary<string, object>)stateData;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string id = saveable.GetType().ToString();
                if (stateBlock.ContainsKey(id))
                {
                    saveable.RestoreState(stateBlock[id]);
                }
            }
        }

        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(uniqueId) || !IsUniqueID(uniqueId))
            {
                uniqueId = System.Guid.NewGuid().ToString();
            }
        }

        public void OnAfterDeserialize()
        {
            // not needed
        }
        private bool IsUniqueID(string id)
        {
            if (!globalLookup.ContainsKey(id)) return true;
            if (!globalLookup[id] == this) return true;
            if (globalLookup[id] == null)
            {
                globalLookup.Remove(id);
                return true;
            }
            if (globalLookup[id].GetUniqueID() != id)
            {
                globalLookup.Remove(id);
                return true;
            }
            return false;
        }
    }
}
