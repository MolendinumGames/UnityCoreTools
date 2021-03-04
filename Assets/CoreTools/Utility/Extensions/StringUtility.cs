using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;

public static class StringUtility
{
    public static string GetLastTypeAsString(this Type type)
    {
        string[] typeHiararchy = type.Name.Split('.');
        int lastId = typeHiararchy.Length - 1;
        return SplitCamelCase(typeHiararchy[lastId]);
    }
    public static string SplitCamelCase(this string str)
    {
        return Regex.Replace(
            Regex.Replace(
                str,
                @"(\P{Ll})(\P{Ll}\p{Ll})",
                "$1 $2"
            ),
            @"(\p{Ll})(\P{Ll})",
            "$1 $2"
        );
    }
}
