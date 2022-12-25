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
            if (value == null)
            {
                Debug.LogWarning($"Tried copying null string to clipboard. Set to empty string instead.");
                GUIUtility.systemCopyBuffer = "";
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