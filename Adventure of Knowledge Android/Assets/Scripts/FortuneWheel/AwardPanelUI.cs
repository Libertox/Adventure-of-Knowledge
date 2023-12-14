using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdventureOfKnowledge.FortuneWheel
{
    public class AwardPanelUI:MonoBehaviour
    {
        [SerializeField] private Button continueButton;
        [SerializeField] private TextMeshProUGUI awardText;

        [SerializeField] private RectTransform panelTransform;

        [SerializeField] private DiamondIndicatorUI diamondIndicatorUI;

        private readonly float animationDuration = 0.5f;

        private void Awake()
        {
            continueButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlayButtonSound();

                panelTransform.DOScale(0, animationDuration).OnComplete(() => Hide());

                diamondIndicatorUI.MoveTowards();
            });   
        }

        private void Start()
        {
            FortuneWheelManager.Instance.OnAwardWon += FortuneWheelManager_OnAwardWon;

            Hide();
        }

        private void FortuneWheelManager_OnAwardWon(object sender, FortuneWheelManager.OnAwardWonEventArgs e) => 
            UpdateAwardText(e.awardAmount);
       

        private void UpdateAwardText(int award)
        {
            Show();
            awardText.SetText(award.ToString());
            panelTransform.DOScale(1, animationDuration);
            diamondIndicatorUI.SetupStartPosition(transform.position);
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

    }
}
