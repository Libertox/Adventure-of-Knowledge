using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.UI
{
    public class PaddlockElementUI:MonoBehaviour
    {
        private const string SHADER_MAIN_TEXTURE_REF = "_MainTexture";
        private const string SHADER_DISSOLVE_AMOUNT_REF = "_DissolveAmount";

        private Image paddlockImage;

        private void Awake()
        {
            paddlockImage = GetComponent<Image>();

            Material material = new Material(paddlockImage.material);
            paddlockImage.material = material;
        }

        private void Start()
        {
            paddlockImage.material.SetTexture(SHADER_MAIN_TEXTURE_REF, paddlockImage.sprite.texture);
        }

        public IEnumerator DissolveCoroutine()
        {
            float dissoloveAmount = 1;
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            while (dissoloveAmount > 0)
            {
                dissoloveAmount -= Time.deltaTime;
                paddlockImage.material.SetFloat(SHADER_DISSOLVE_AMOUNT_REF, dissoloveAmount);
                yield return waitForEndOfFrame;
            }
            SetActive(false);
        }

        public void SetActive(bool active) => gameObject.SetActive(active); 

    }
}
