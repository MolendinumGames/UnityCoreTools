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

        private void Awake()
        {
            startSize = transform.localScale;
        }

        public void StartAnimation()
        {
            transform.DOKill(); // remove any tweens targeting this transform

            transform.localScale = Vector3.zero;
            transform.DOScaleX(startSize.x, popupDuration);
            transform.DOScaleY(startSize.y, popupDuration);
        }
    }
}