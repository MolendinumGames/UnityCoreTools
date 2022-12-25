/* Copyright (c) 2022 - Christoph Römer. All rights reserved. 
 * 
 * This source code is licensed under the Apache-2.0-style license found
 * in the LICENSE file in the root directory of this source tree. 
 * You may not use this file except in compliance with the License.
 * 
 * For questions, feedback and suggestions please conact me under:
 * coretools@molendinumgames.com
 */

using UnityEngine;

namespace CoreTools
{
    public static class TransformUtility
    {
        public static void ResetPosition(this Transform t) => t.position = Vector3.zero;
        public static void ResetLocalPosition(this Transform t) => t.localPosition = Vector3.zero;

        public static void ResetRotation(this Transform t) => t.rotation = Quaternion.identity;
        public static void ResetLocalRotation(this Transform t) => t.localRotation = Quaternion.identity;

        public static void ResetScale(this Transform t) => t.localScale = Vector3.one;
        public static void ResetLocalScale(this Transform t) => t.localScale = Vector3.one;

        /// <summary>
        /// Swap both transforms positions
        /// </summary>
        public static void SwapPosition(this Transform current, Transform target)
        {
            Vector3 pos1 = current.position;
            Vector3 pos2 = target.position;
            current.position = pos2;
            target.position = pos1;
        }
    }
}