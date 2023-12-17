
using System.Text;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

namespace AdventureOfKnowledge.FortuneWheel
{
    public class SpinTimerUI:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI spinTimerText;

        private StringBuilder spinTextBuilder = new StringBuilder();

        private void Awake()
        {
            SpinTimer.OnSpinTimeChanged += SpinTimer_OnSpinTimeChanged;
            SpinTimer.OnCanSpined += SpinTimer_OnCanSpined;          
        }

        private void Start() 
        {
            FortuneWheelManager.Instance.OnAwardWon += FortuneWheelManager_OnAwardWon;
            AdsManager.Instance.OnRewardedAdsGet += AdsManager_OnRewarded;

            gameObject.SetActive(false);
        }

        private void AdsManager_OnRewarded(object sender, System.EventArgs e)
        {
            Hide();
        }

        private void SpinTimer_OnCanSpined(object sender, SpinTimer.OnCanSpinedEventArgs e)
        {
            gameObject.SetActive(!e.canSpined);
        }

        private void FortuneWheelManager_OnAwardWon(object sender, FortuneWheelManager.OnAwardWonEventArgs e)
        { 
            Show();
        }

        private void SpinTimer_OnSpinTimeChanged(object sender, SpinTimer.OnSpinTimeChangedEventArgs e)
        {
            UpdateSpinTimer(e.hour,e.minute,e.second);
        }

        private void UpdateSpinTimer(int hour, int minute, int second)
        {
            const int unitDignit = 10;

            if (hour < unitDignit) spinTextBuilder.Append($"0{hour}:");
            else spinTextBuilder.Append($"{hour}:");

            if (minute < unitDignit) spinTextBuilder.Append($"0{minute}:");
            else spinTextBuilder.Append($"{minute}:");

            if (second < unitDignit) spinTextBuilder.Append($"0{second}");
            else spinTextBuilder.Append($"{second}");

            spinTimerText.SetText(spinTextBuilder.ToString());

            spinTextBuilder.Clear();
        }

        private void Show() => gameObject.SetActive(true);

        private void Hide() => gameObject.SetActive(false);

        private void OnDestroy()
        {
            SpinTimer.OnSpinTimeChanged -= SpinTimer_OnSpinTimeChanged;
            SpinTimer.OnCanSpined -= SpinTimer_OnCanSpined;
        }

    }


}
