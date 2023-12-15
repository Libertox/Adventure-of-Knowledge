using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.FortuneWheel
{

    public class FortuneWheelUI : MonoBehaviour
    {
        private const float WAIT_TIME_AFTER_SPIN = 0.5f;

        [SerializeField] private float minSpinTime = 5f;
        [SerializeField] private float maxSpinTime = 9f;
        [SerializeField] private float minRotationSpeed = 0.1f;
        [SerializeField] private float maxRotationSpeed = 1f;

        [SerializeField] private FortuneWheelElementUI elementPrefab;

        [SerializeField] private RectTransform fortuneElementContainer;

        [SerializeField] private Button spinButton;
 
        private bool isSpin;

        private void Awake()
        {
            SpinTimer.OnCanSpined += SpinTimer_OnCanSpined;

            spinButton.onClick.AddListener(() => 
            {
                if (!isSpin) 
                {
                    SoundManager.Instance.PlaySpinningWheelSound();
                    isSpin = true;
                    StartCoroutine(SpinCorotuine());
                } 
            });
          
        }

        private void Start() 
        {
            CreateFortuneWheel();
            AdsManager.Instance.OnRewardedAdsGet += AdsManager_OnRewarded;
        }

        private void AdsManager_OnRewarded(object sender, System.EventArgs e)
        {
            spinButton.gameObject.SetActive(true);
            isSpin = false;
        }

        private void SpinTimer_OnCanSpined(object sender, SpinTimer.OnCanSpinedEventArgs e)
        {
            spinButton.gameObject.SetActive(e.canSpined);
        }

        private void CreateFortuneWheel()
        {
            int elementNumber = FortuneWheelManager.Instance.GetNumberOfWheelElement();
            float anglePerElement = 360 / elementNumber;
            float lengthElement = (1f / elementNumber);
            float startAngle = elementPrefab.transform.eulerAngles.z;
            fortuneElementContainer.eulerAngles = new Vector3(0f, 0f, anglePerElement * 0.5f);


            for (int i = 0; i < elementNumber; i++)
            {
                FortuneWheelElementUI newElement = Instantiate(elementPrefab, fortuneElementContainer);
                newElement.UpdateFillAmount(lengthElement);
                newElement.UpdateVisual(FortuneWheelManager.Instance.GetFortuneWheelElementForIndex(i));
                newElement.UpdateAngel(startAngle, anglePerElement);
                startAngle += anglePerElement;
            }
        }

        private IEnumerator SpinCorotuine()
        {
            float spinTime = Random.Range(minSpinTime, maxSpinTime);
            float rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);
            float curentSpinTime = 0;
    
            while (rotationSpeed > 0)
            {
                curentSpinTime += Time.deltaTime;
              
                fortuneElementContainer.Rotate(new Vector3(0, 0, rotationSpeed), Space.Self);

                rotationSpeed += curentSpinTime > spinTime ? -Time.deltaTime : Time.deltaTime;

                yield return null;
            }

            SoundManager.Instance.StopSound();

            yield return new WaitForSeconds(WAIT_TIME_AFTER_SPIN);

            float finishAngle = AlignWheelIndicator();
           
            CalculateAwardIndex(finishAngle);
            spinButton.gameObject.SetActive(false);
        }

       
        private float AlignWheelIndicator()
        {
            float anglePerElement = 360 / FortuneWheelManager.Instance.GetNumberOfWheelElement();
            float halfAngelPerElement = anglePerElement * 0.5f;
            float angleLeftBound = (int)(fortuneElementContainer.eulerAngles.z / halfAngelPerElement) * halfAngelPerElement;

            if (angleLeftBound % anglePerElement == 0)
                angleLeftBound += halfAngelPerElement;

            float angleRightBound = angleLeftBound + anglePerElement;
            float finishAngle = Mathf.Abs(angleLeftBound - fortuneElementContainer.eulerAngles.z) > Mathf.Abs(angleRightBound - fortuneElementContainer.eulerAngles.z) ? angleRightBound : angleLeftBound;
            fortuneElementContainer.DORotate(new Vector3(0, 0, finishAngle), 1f);

            return finishAngle;
        }

        private static void CalculateAwardIndex(float finishAngle)
        {
            float anglePerElement = 360 / FortuneWheelManager.Instance.GetNumberOfWheelElement();
            float halfAngelPerElement = anglePerElement * 0.5f;
            int elementIndex = (int)(finishAngle / anglePerElement);

            if (finishAngle < 0)
                elementIndex++;
            else if (finishAngle > halfAngelPerElement)
                elementIndex = FortuneWheelManager.Instance.GetNumberOfWheelElement() - elementIndex;
 
            FortuneWheelManager.Instance.SetAward(elementIndex);
        }

        private void OnDestroy() => SpinTimer.OnCanSpined -= SpinTimer_OnCanSpined;
    }

    
}