using System;
using UnityEngine;

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
            Debug.LogWarning($"Copied null string to clipboard.");
            GUIUtility.systemCopyBuffer = "";
            return;
        }
        GUIUtility.systemCopyBuffer = value;
    }

    /// <summary>
    /// Copy a string to system wide clipboard
    /// </summary>
    public static void CopyToClipboard(this string value) => SetClipboard(value);
}
