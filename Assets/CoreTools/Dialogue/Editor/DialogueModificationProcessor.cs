using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using CoreTools.Dialogue;

namespace CoreTools.Dialogue.Editor
{
    public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            DialogueSO dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as DialogueSO;
            if (dialogue == null)
            {
                return AssetMoveResult.DidNotMove;
            }

            if (Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath))
            {
                return AssetMoveResult.DidNotMove;
            }

            dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);

            return AssetMoveResult.DidNotMove;
        }
    }
}