using System;
using UnityEngine;

namespace AdventureOfKnowledge.FortuneWheel
{
    public class SpinTimer:MonoBehaviour
    {
        public static event EventHandler<OnSpinTimeChangedEventArgs> OnSpinTimeChanged;
        public static event EventHandler<OnCanSpinedEventArgs> OnCanSpined;

        private OnSpinTimeChangedEventArgs onSpinTimeChangedEventArgs;

        public class OnSpinTimeChangedEventArgs : EventArgs
        {
            public int hour;
            public int minute;
            public int second;

            public void SetTime(int hour, int minute, int second)
            {
                this.hour = hour;
                this.minute = minute;
                this.second = second;
            }
        }

        public class OnCanSpinedEventArgs:EventArgs { public bool canSpined; };

        private void Awake() => onSpinTimeChangedEventArgs = new OnSpinTimeChangedEventArgs();
      
        private void Start()
        {
            FortuneWheelManager.Instance.OnAwardWon += FortuneWheelManager_OnAwardWon;
            AdsManager.Instance.OnRewardedAdsShow += AdsManager_OnRewardedAdsShow;

            SaveManager.LoadSpinTime((callback) =>
            {
                CurrentDate currentDate = new CurrentDate();
                if (callback.Value != null)
                {
                    currentDate = JsonUtility.FromJson<CurrentDate>(callback.Value.ToString());
                }

                bool canSpined = !currentDate.CheckItTheCurrentDate();
                OnCanSpined?.Invoke(this, new OnCanSpinedEventArgs
                {
                    canSpined = canSpined
                });
                gameObject.SetActive(!canSpined);

            });
        }

        private void AdsManager_OnRewardedAdsShow(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void FortuneWheelManager_OnAwardWon(object sender, FortuneWheelManager.OnAwardWonEventArgs e)
        {
            gameObject.SetActive(true);
        }

        private void Update()
        {
            int hour = DateTime.MaxValue.Hour - DateTime.Now.Hour;
            int minute = DateTime.MaxValue.Minute - DateTime.Now.Minute;
            int second = DateTime.MaxValue.Second - DateTime.Now.Second;

            onSpinTimeChangedEventArgs.SetTime(hour, minute, second);

            if(CheckSpinTimerElaspsed(hour, minute, second))
            {
                OnCanSpined?.Invoke(this, new OnCanSpinedEventArgs
                {
                    canSpined = true
                });
                gameObject.SetActive(false);
            }

            OnSpinTimeChanged?.Invoke(this, onSpinTimeChangedEventArgs);
        }

      

        private bool CheckSpinTimerElaspsed(int hour, int minute, int second) => hour == 0 && minute == 0 && second == 0;

    }
}
