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
using DG.Tweening;

namespace CoreTools.UI
{
    public class TooltipAnimator : MonoBehaviour
    {
        [Range(0f, .5f)]
        [SerializeField]
        float popupDuration = .2f;

        Vector3 startSize;

        private void Awake() =>
            startSize = transform.localScale;

        private void OnDisable() =>
            transform.DOKill();

        public void StartAnimation()
        {
            transform.DOKill(); // remove any tweens targeting this transform

            transform.localScale = Vector3.zero;
            transform.DOScaleX(startSize.x, popupDuration);
            transform.DOScaleY(startSize.y, popupDuration);
        }
    }
}