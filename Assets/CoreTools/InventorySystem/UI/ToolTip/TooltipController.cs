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

        private void Awake()
        {
            anim = GetComponent<TooltipAnimator>();
        }

        public void Initialitze(string header, string body)
        {
			headerText.text = header;
			bodyText.text = body;

            if (anim != null)
			    StartAnimation();
        }

        private void StartAnimation()
        {
            anim.StartAnimation();
        }
    }	
}
