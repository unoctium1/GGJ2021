using System;
using TMPro;
using UnityEngine;

namespace GameJamCat
{
    public class TimerUI : MonoBehaviour
    {
        private const string TimeSpanFormat = "mm\\:ss";
        [SerializeField] private TextMeshProUGUI _timerText = null;

        public void Initialize()
        {
            CleanUp();
        }

        public void UpdateTime(float time)
        {
            SetTimeUI(time);
        }

        private void CleanUp()
        {
            SetTimeUI(0f);
        }

        private void SetTimeUI(float time)
        {
            if (_timerText != null)
            {
                _timerText.text = TimeSpan.FromSeconds(time).ToString(TimeSpanFormat);
            }
        }
    }
}
