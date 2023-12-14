using AdventureOfKnowledge.FortuneWheel;
using TMPro;
using UnityEngine;

namespace AdventureOfKnowledge.UI
{
    public class DiamondNumberTextUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI diamondNumberText;

        private void Awake()
        {
            PlayerDiamond.OnDiamondChanged += PlayerDiamond_OnDiamondChanged;
        }

        private void PlayerDiamond_OnDiamondChanged(object sender, PlayerDiamond.OnDiamondChangedEventArgs e)
        {
            UpdateDiamondNumber(e.diamondAmount);
        }

        private void UpdateDiamondNumber(int diamondAmount)
        {
            diamondNumberText.SetText(diamondAmount.ToString());
        }

        private void OnDestroy()
        {
            PlayerDiamond.OnDiamondChanged -= PlayerDiamond_OnDiamondChanged;
        }

    }
}
