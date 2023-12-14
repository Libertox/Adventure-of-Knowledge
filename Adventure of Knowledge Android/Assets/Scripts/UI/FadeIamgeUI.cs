using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventureOfKnowledge.UI
{
    public class FadeIamgeUI:MonoBehaviour
    {
        private CanvasGroup canvasGroup;
        [SerializeField] private bool fadeOnAwake;


        private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

        private void Start()
        {
            if (fadeOnAwake)
                StartCoroutine(FadeFromBlackCorotuine(() => Hide()));
            else
                Hide();
        }

        private void Hide() => gameObject.SetActive(false);

        private void Show() => gameObject.SetActive(true);

        public void FadeToBlack(Action funcAfterFade)
        {
            Show();
            StartCoroutine(FadeToBlackCorotuine(funcAfterFade));
        }

        public void FadeFromBlack(Action funcAfterFade)
        {
            Show();
            StartCoroutine(FadeFromBlackCorotuine(funcAfterFade));
        }


        private IEnumerator FadeToBlackCorotuine(Action funcAfterFade)
        {
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime);
                yield return waitForEndOfFrame;
            }

            MusicManager.Instance.SaveClipTime();
            funcAfterFade();
        }

        private IEnumerator FadeFromBlackCorotuine(Action funcAfterFade)
        {
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime);
                yield return waitForEndOfFrame;
            }
            funcAfterFade();
        }

    }
}
