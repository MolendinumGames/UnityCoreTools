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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using CoreTools;
using UnityEngine.SceneManagement;

namespace CoreTools.SaveSystem
{
	public class SavingManager : Singleton<SavingManager>
	{
        protected override bool Persistent => true;

        private const string lastSceneKey = "lastSceneBuildIndex";

        protected override void Awake()
        {
            base.Awake();
        }

        protected List<SaveableEntity> saveEntities = new List<SaveableEntity>();

        #region Public
        public void Save(string saveFile)
        {
            // Load already existing save file otherwise create a new one
            Dictionary<string, object> saveState = LoadFile(saveFile);

            // Fill the dictionary with all saveable entities currently active in all scenes
            CaptureSaveState(saveState);

            // Overwrite the savefile
            SaveFile(saveFile, saveState);
        }

        public void LoadLastSave(string saveFile) =>
            StartCoroutine(LoadLastScene(saveFile));

        public void ApplyLastSaveState(string saveFile) =>
            RestoreSaveState(LoadFile(saveFile));

        public void RegisterSaveEntitiy(SaveableEntity e)
        {
            if (!saveEntities.Contains(e))
                saveEntities.Add(e);
        }

        public void DeregisterSaveEntity(SaveableEntity e)
        {
            if (saveEntities.Contains(e))
                saveEntities.Remove(e);
        }
        #endregion

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                Debug.LogError($"Trying to load from nonexisting savefile: {saveFile}");
                return new Dictionary<string, object>();
            }
            Debug.Log($"Loading saveile: {saveFile}");
            using FileStream stream = File.Open(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
        private void SaveFile(string saveFile, object saveState)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path}");

            using FileStream stream = File.Open(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, saveState);
        }

        private void CaptureSaveState(Dictionary<string, object> saveState)
        {
            foreach (SaveableEntity saveable in saveEntities)
            {
                if (saveable.enabled)
                    saveState[saveable.UniqueID] = saveable.CaptureStates();
            }
            StoreSceneIndex(saveState);
        }
        private void RestoreSaveState(Dictionary<string, object> saveState)
        {
            foreach (SaveableEntity saveable in saveEntities)
            {
                if (saveable.enabled)
                {
                    string id = saveable.UniqueID;
                    if (saveState.ContainsKey(id))
                        saveable.RestoreStates(saveState[id]);
                }
            }
        }
        private IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey(lastSceneKey))
                buildIndex = (int)state[lastSceneKey];
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreSaveState(state);
        }
        private void StoreSceneIndex(Dictionary<string, object> saveState) =>
            saveState[lastSceneKey] = SceneManager.GetActiveScene().buildIndex;

        private string GetPathFromSaveFile(string saveFile) =>
            Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }	
}
