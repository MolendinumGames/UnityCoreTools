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
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

namespace CoreTools
{
    public static class StringUtility
    {
        /// <summary>
        /// Get the type of an object without the type hierarchy as string
        /// </summary>
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
        // Example: "<b>Hello World!</b>" => "Hello World!"
        public static string StripHTMLLazy(string s) =>
            Regex.Replace(s, "<[^ ]*?>", String.Empty);

        /// <summary>
        /// Get n-times the input string
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