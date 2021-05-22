using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreTools
{
	public static class ListUtility
	{
		/// <summary>
		/// Randomizes list and changes positions permanently.
		/// </summary>
		public static void Shuffle<T>(this List<T> list)
        {
			int n = list.Count;
			while (n > 1)
            {
				n--;
				int randomIndex = UnityEngine.Random.Range(0, n + 1);
				T storedValue = list[randomIndex];
				list[randomIndex] = list[n];
				list[n] = storedValue;
            }
        }
		/// <summary>
		/// Reinsert item from one index to another.
		/// </summary>
		/// <param name="old">The current item index</param>
		/// <param name="target">the new position for the item</param>
		public static void MoveToIndex<T>(this List<T> list, int old, int target)
        {
			T item = list[old];
			list.RemoveAt(old);
			list.Insert(target, item);
        }
	}	
}
