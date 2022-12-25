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
using CoreTools;

namespace CoreTools.SaveSystem
{
    // Attach to the Gameobject with ISaveable components
    public class SaveableEntity : MonoBehaviour, ISerializationCallbackReceiver
    {
        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        public string UniqueID { get; private set; } = "";

        private void OnEnable() => SavingManager.Instance.RegisterSaveEntitiy(this);
        private void OnDisable() => SavingManager.Instance?.DeregisterSaveEntity(this);

        /// <summary>
        /// Read all states through ISaveable from components on this GO
        /// </summary>
        /// <returns>Dictionary of ID and savedata as object</returns>
        public object CaptureStates()
        {
            var stateBlock = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                stateBlock[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return stateBlock;
        }
        /// <summary>
        /// Feed the state data to all ISaveable Components on the GO
        /// </summary>
        /// <param name="stateData">Dictionary of ID and savedata as object</param>
        public void RestoreStates(object stateData)
        {
            var stateBlock = (Dictionary<string, object>)stateData;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string id = saveable.GetType().ToString();
                if (stateBlock.ContainsKey(id))
                {
                    saveable.RestoreState(stateBlock[id]);
                }
            }
        }

        #region Serialization
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(UniqueID) || !IsUniqueID(UniqueID))
            {
                UniqueID = System.Guid.NewGuid().ToString();
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
            if (globalLookup[id].UniqueID != id)
            {
                globalLookup.Remove(id);
                return true;
            }
            return false;
        }
        #endregion
    }
}
