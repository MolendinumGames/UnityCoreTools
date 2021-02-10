using UnityEngine;

namespace EasyAudioSystem.Utility
{
    /* This Utility class provides extension methods for strings.
     * If you want to use these methods in your own projects simply
     * inlude the namespace at the top:
     * 'using EasyAudioSystem.Utility'
     */
    /// <summary>
    /// Extension functions for string manipulation.
    /// </summary>
    public static class StringUtility
    {
        /// <summary>
        /// Get only the numbers as string back.
        /// </summary>
        /// <param name="w"></param>
        /// <returns>Numbers as string.</returns>
        public static string GetNumbersOnly(this string w)
        {
            string returnString = "";
            for (int i = 0; i < w.Length; i++)
            {
                if (char.IsDigit(w[i])) returnString += w[i];
            }
            return returnString;
        }
        /// <summary>
        /// Set the string to lower case except the first character to upper.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharIsUpper(this string input)
        {
            if (input == null) return null;
            if (input == "") return "";

            input.ToLower();
            return input[0].ToString().ToUpper() + input.Substring(1);
        }
        public static string CamelCaseToDescription(this string s)
        {
            return System.Text.RegularExpressions.Regex.Replace(s, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().FirstCharIsUpper();
        }
    }
}
