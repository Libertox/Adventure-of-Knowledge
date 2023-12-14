using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace AdventureOfKnowledge.FortuneWheel
{
    public class FortuneWheelElementUI:MonoBehaviour
    {
        [SerializeField] private Image fortuneWheelElementImage;
        [SerializeField] private RectTransform iconTransform;
        [SerializeField] private TextMeshProUGUI awardValueText;

      
        public void UpdateVisual(FortuneWheelElement fortuneWheelElement)
        {
            awardValueText.SetText($"{fortuneWheelElement.AwardValue}");
            fortuneWheelElementImage.color = fortuneWheelElement.ElementColor;
        }

        public void UpdateAngel(float angle , float anglePerElement)
        {
            float anglerPerElementHalf = anglePerElement * 0.5f;

            transform.localEulerAngles = new Vector3(0, 0, angle);
            iconTransform.transform.localEulerAngles = new Vector3(0, 0, -anglerPerElementHalf);
            awardValueText.transform.localEulerAngles = new Vector3(0, 0, -anglerPerElementHalf);       
        }

        public void UpdateFillAmount(float fillAmount) 
        {
            float iconPositionOffset = fillAmount * 560;
            float awardPositionOffset = fillAmount * 300;

            fortuneWheelElementImage.fillAmount = fillAmount;
            
            iconTransform.anchoredPosition = new Vector2(iconTransform.anchoredPosition.x + iconPositionOffset, iconTransform.anchoredPosition.y);
            awardValueText.rectTransform.anchoredPosition = new Vector2(awardValueText.rectTransform.anchoredPosition.x + awardPositionOffset, awardValueText.rectTransform.anchoredPosition.y);
        } 

    }
}
