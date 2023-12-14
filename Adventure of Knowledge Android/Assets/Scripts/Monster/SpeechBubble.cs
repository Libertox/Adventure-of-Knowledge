using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace AdventureOfKnowledge
{
    public class SpeechBubble:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI speechBubbleText;
        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private float writeCharacterInterval;
        [SerializeField] private float lifeTime;

        protected bool isWriting;
       
        protected IEnumerator SlowlyWriteDialog(string word)
        {
            isWriting = true;
            StringBuilder stringBuilder = new StringBuilder();
            WaitForSeconds waitForSeconds = new WaitForSeconds(writeCharacterInterval);

            yield return new WaitUntil(() => Show());
           
            foreach (char character in word)
            {
                stringBuilder.Append(character);
                speechBubbleText.SetText(stringBuilder.ToString());
                yield return waitForSeconds;
            }
            yield return new WaitForSeconds(lifeTime);

            yield return new WaitUntil(() => Hide());

            speechBubbleText.SetText("");

            isWriting = false;
           
        }

        private bool Show()
        {
            float transparencySpriteBoundry = 1f;
            float transparencyTextBoundry = 1f;

            while (speechBubbleText.color.a < transparencyTextBoundry)
            {
                spriteRenderer.color = GetNewColor(spriteRenderer.color, transparencySpriteBoundry, Time.deltaTime);

                speechBubbleText.color = GetNewColor(speechBubbleText.color, transparencyTextBoundry, Time.deltaTime);
                return false;
            }
            return true;
        }

        private bool Hide()
        {
            float transparencyBoundry = 0;
            float changeTransparencyTextSpeed = speechBubbleText.color.a / spriteRenderer.color.a;
            while (speechBubbleText.color.a > transparencyBoundry)
            {
                spriteRenderer.color = GetNewColor(spriteRenderer.color, transparencyBoundry, Time.deltaTime);

                speechBubbleText.color = GetNewColor(speechBubbleText.color, transparencyBoundry, Time.deltaTime * changeTransparencyTextSpeed);

                return false;
            }
            return true;
        }

        private Color GetNewColor(Color currentColor, float transparencyBoundry , float changeColorSpeed)
        {
            return new Color(
                    currentColor.r,
                    currentColor.g,
                    currentColor.b,
                    Mathf.MoveTowards(currentColor.a, transparencyBoundry, changeColorSpeed));
        }
    }
}
