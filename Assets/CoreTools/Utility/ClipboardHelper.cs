/* Copyright (c) 2021 - Christoph Römer. All rights reserved. 
 * 
 * For support, feedback and suggestions please conact me under:
 * contactsundiray@gmail.com
 * 
 * Check out my other content:
 * https://sundiray.itch.io/
 */

using System;
using UnityEngine;

namespace CoreTools
{
    public static class ClipboardHelper
    {
        /// <returns> The systems clipboard as string</returns>
        public static string ReadClipboard() => GUIUtility.systemCopyBuffer;

        /// <summary>
        /// Copy a string to system wide clipboard
        /// </summary>
        public static void SetClipboard(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogWarning($"Copied empty string to clipboard.");
                GUIUtility.systemCopyBuffer = "";
                return;
            }
            else
            {
                GUIUtility.systemCopyBuffer = value;
            }
        }

        /// <summary>
        /// Copy a string to system wide clipboard
        /// </summary>
        public static void CopyToClipboard(this string value) => SetClipboard(value);
    }
}