/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using UnityEngine;
using UnityEditor;

namespace CoreTools.Console
{
    // Provides functionality to create and select a DeveloperConsole into the currently acitve scene.
    // If an instance of a DeveloperConsole is already in the scene, the root GameObject will be selected instead.
    public static class DevConsoleEditorUtility
    {
        const string prefabPath = "ConsolePrefabs/DeveloperConsole";

        [MenuItem("Tools/Add Developer Console")]
        public static void DevConsoleCreator()
        {
            // CoreTools.Console.DeveloperConsole is not a MonoBehaviour so we search for the Controller instead.
            // The DevConsoleController sits in the root GameObject of the DevConsole GameObject. 
            DevConsoleController devConsole = GameObject.FindObjectOfType<DevConsoleController>();

            if (devConsole != null)
            {
                Selection.activeGameObject = devConsole.gameObject;
            }
            else
            {
                CreateNewDevConsole();
            }
        }

        static void CreateNewDevConsole()
        {
            var prefab = LoadDevConsolePrefab();
            var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, null);
            go.name = prefab.name;
            Selection.activeGameObject = go;
        }

        static GameObject LoadDevConsolePrefab()
        {
            var prefab = Resources.Load<GameObject>(prefabPath);

            if (prefab == null)
                throw new System.NullReferenceException($"Developer console prefab not found in resource path: {prefabPath}");

            return prefab;
        }
    }
}