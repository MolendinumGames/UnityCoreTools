using System;
using System.Reflection;
using UnityEngine;

public static class ClipboardHelper
{
    public static string ReadClipboard()
    {
        return GUIUtility.systemCopyBuffer;
    }
    public static void SetClipboard(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogWarning($"Tried to copy an empty string to clipboard. String: {value}");
            return;
        }
        GUIUtility.systemCopyBuffer = value;
    }
    public static void CopyToClipboard(this string value) => SetClipboard(value);
}
