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
using TMPro;

namespace CoreTools.UI
{
	public class TooltipController : MonoBehaviour
	{
		[SerializeField]
		TextMeshProUGUI headerText;
		[SerializeField]
		TextMeshProUGUI bodyText;

		TooltipAnimator anim;

        private void Awake()
        {
            anim = GetComponent<TooltipAnimator>();
        }

        public void Initialitze(string header, string body)
        {
			headerText.text = header;
			bodyText.text = body;

            if (anim != null)
			    anim.StartAnimation();
        }
    }	
}
