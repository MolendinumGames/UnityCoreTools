using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CoreTools.Console
{
    public class ConsoleScroller : MonoBehaviour
    {
        Scrollbar scrollbar;

        private void Awake() => scrollbar = GetComponent<Scrollbar>();

        public void MoveDown() => StartCoroutine(ResetScroll());

        IEnumerator ResetScroll()
        {
            yield return new WaitForEndOfFrame();
            if (scrollbar)
                scrollbar.value = 0;
        }
    }
}