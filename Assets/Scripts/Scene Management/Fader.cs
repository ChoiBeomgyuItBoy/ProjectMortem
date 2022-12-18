using System.Collections;
using UnityEngine;

namespace Mortem.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup canvasGroup;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float time)
        {
            if(canvasGroup.alpha >= 1) yield break;

            while(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return new WaitForEndOfFrame();
            }
        }

        public IEnumerator FadeIn(float time)
        {
            if(canvasGroup.alpha <= 0) yield break;

            while(canvasGroup.alpha > 0)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
