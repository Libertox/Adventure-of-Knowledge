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
        [SerializeField] private float fadeFromBlackSpeed;


        private void Awake() 
        {
            canvasGroup = GetComponent<CanvasGroup>();
            SaveManager.OnLoadCompleted += SaveManager_OnLoadCompleted;
        }

        private void SaveManager_OnLoadCompleted()
        {
            if (fadeOnAwake)
                StartCoroutine(FadeFromBlackCorotuine(() => Hide()));
        }

     
        public void Hide() => gameObject.SetActive(false);

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
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1f, Time.deltaTime);
                yield return null;
            }

            MusicManager.Instance.SaveClipTime();
            funcAfterFade();
        }

        private IEnumerator FadeFromBlackCorotuine(Action funcAfterFade)
        {
            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0f, Time.deltaTime * fadeFromBlackSpeed);
                yield return null;
            }
            funcAfterFade();
        }

        private void OnDestroy()
        {
            SaveManager.OnLoadCompleted -= SaveManager_OnLoadCompleted;
        }

    }
}
