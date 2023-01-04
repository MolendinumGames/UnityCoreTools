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
using UnityEngine;
using UnityEngine.UI;

namespace CoreTools.Console
{
    public class ConsoleScroller : MonoBehaviour
    {
        Scrollbar scrollbar;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        bool isClosing = false;

        private void Awake() => scrollbar = GetComponent<Scrollbar>();

        private void OnEnable() => isClosing = false;

        private void OnDisable() => isClosing = true;

        public void MoveDown()
        {
            if (!isClosing)
            {
                StartCoroutine(ResetScroll());
            }
        }

        IEnumerator ResetScroll()
        {
            yield return waitForEndOfFrame;
            scrollbar.value = 0;
        }
    }
}