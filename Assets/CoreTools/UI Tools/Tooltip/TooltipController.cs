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
