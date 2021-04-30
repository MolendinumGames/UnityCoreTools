using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using CoreTools.UI;

namespace CoreTools.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
	public class TooltipController : MonoBehaviour
	{
		[SerializeField]
		TextMeshProUGUI headerText;
		[SerializeField]
		TextMeshProUGUI bodyText;

		TooltipAnimator anim;

        public void Initialitze(string header, string body)
        {
			headerText.text = header;
			bodyText.text = body;
			StartAnimation();
        }

        private void StartAnimation()
        {
			if (anim == null)
				anim = GetComponent<TooltipAnimator>();
			//if (anim != null)

        }
    }	
}
