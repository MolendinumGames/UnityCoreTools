using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using CoreTools;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
	public class SavingManager : Singleton<SavingManager>
	{
        protected override bool Persistent => true;

        private readonly string lastSceneKey = "lastSceneBuildIndex";


        #region EventFunctions
        protected override void OnAwake()
        {
            
        }
        #endregion

        #region Public
        public void Save(string saveFile)
        {
            Dictionary<string, object> saveState = LoadFile(saveFile);
            CaptureSaveState(saveState);
            SaveFile(saveFile, saveState);
        }
        public void LoadLastSave(string saveFile)
        {
            StartCoroutine(LoadLastScene(saveFile));
        }
        public void ApplyLastSaveState(string saveFile)
        {
            RestoreSaveState(LoadFile(saveFile));
        }
        #endregion
        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
                return new Dictionary<string, object>();
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }
        private void SaveFile(string saveFile, object saveState)
        {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path}");
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, saveState);
            }
        }

        private void CaptureSaveState(Dictionary<string, object> saveState)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                saveState[saveable.GetUniqueID()] = saveable.CaptureStates();
            }
            StoreSceneIndex(saveState);
        }
        private void RestoreSaveState(Dictionary<string, object> saveState)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueID();
                if (saveState.ContainsKey(id))
                    saveable.RestoreStates(saveState[id]);
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
        private void StoreSceneIndex(Dictionary<string, object> saveState) => saveState[lastSceneKey] = SceneManager.GetActiveScene().buildIndex;

        private string GetPathFromSaveFile(string saveFile) => Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }	
}
