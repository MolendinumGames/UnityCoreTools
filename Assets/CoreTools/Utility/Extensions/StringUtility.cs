using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.Text;

namespace CoreTools
{
    public static class StringUtility
    {
        public static string GetLastTypeAsString(this Type type)
        {
            string[] typeHiararchy = type.Name.Split('.');
            int lastId = typeHiararchy.Length - 1;
            return SplitCamelCase(typeHiararchy[lastId]);
        }
        public static string SplitCamelCase(this string s)
        {
            return Regex.Replace(
                     Regex.Replace(s, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"),
                                         @"(\p{Ll})(\P{Ll})", "$1 $2");
        }
        /// <summary>
        /// Clears a string from HTML/Rich-Tags
        /// </summary>
        /// <param name="s">String to be cleared</param>
        // Example: "<b>Hello World!</b>" => "Hello World!"
        public static string StripHTMLLazy(this string s) =>
            Regex.Replace(s, "<[^ ]*?>", String.Empty);

        /// <summary>
        /// N-times the input string s as new string
        /// </summary>
        public static string Multiply(this string s, int n)
        {
            var builder = new StringBuilder();
            for (int i = 0; i < n; i++)
                builder.Append(s);
            return builder.ToString();
        }
    }

}