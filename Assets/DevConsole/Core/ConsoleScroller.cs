using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CoreTools.Console
{
    public class ConsoleScroller : MonoBehaviour
    {
        Scrollbar scrollbar;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private void Awake() => scrollbar = GetComponent<Scrollbar>();

        public void MoveDown() => StartCoroutine(ResetScroll());

        IEnumerator ResetScroll()
        {
            yield return waitForEndOfFrame;
            if (scrollbar)
                scrollbar.value = 0;
        }
    }
}