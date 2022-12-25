/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

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
		/// Set index of object by reinserting into the list.
		/// </summary>
		/// <param name="old">Current index of the object</param>
		/// <param name="target">New position of the object</param>
		public static void MoveToIndex<T>(this List<T> list, int old, int target)
        {
			T item = list[old];
			list.RemoveAt(old);
			list.Insert(target, item);
        }
	}	
}
