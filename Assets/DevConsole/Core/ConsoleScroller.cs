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
                StartCoroutine(ResetScroll());
        }

        IEnumerator ResetScroll()
        {
            yield return waitForEndOfFrame;
            if (scrollbar)
                scrollbar.value = 0;
        }
    }
}