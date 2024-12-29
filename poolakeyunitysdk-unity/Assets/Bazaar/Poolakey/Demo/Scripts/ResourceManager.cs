using System;
using System.Collections;
using System.Globalization;
using UnityEngine;

namespace PoolakeyDemo
{
    public class ResourceManager : MonoBehaviour
    {
        
        const string StarCountKey = "StarCountKey";
        const string JellyEndTimeKey = "JellyEndTimeKey";
        public Action<int> OnStarCountChange;
        public Action<string> OnRemainingTimeChange;
        private DateTime _endTime;
        private int _starCount;
        private Coroutine _timerCoroutine;

        public void AddStar(int starCount)
        {
            _starCount += starCount;
            PlayerPrefs.SetInt(StarCountKey, _starCount);
            OnStarCountChange.Invoke(_starCount);
        }

        public void AddJellyEndTime(TimeSpan jellyTime)
        {
            if (_endTime > DateTime.UtcNow)
            {
                _endTime += jellyTime;
            }
            else
            {
                _endTime = DateTime.UtcNow + jellyTime;
            }
            PlayerPrefs.SetString(JellyEndTimeKey, _endTime.ToString("o", CultureInfo.InvariantCulture));
    
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
            }
            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        private void Start()
        {
            _starCount = GetStarCount();
            _endTime = GetEndRemainingTime();
            OnStarCountChange?.Invoke(_starCount);
            _timerCoroutine=StartCoroutine(TimerCoroutine());
        }

        private int GetStarCount()
        {
            return PlayerPrefs.GetInt(StarCountKey, 0);
        }

        private DateTime GetEndRemainingTime()
        {
            var rawEndTime = PlayerPrefs.GetString(JellyEndTimeKey, DateTime.MinValue.ToString("o", CultureInfo.InvariantCulture));
            DateTime.TryParseExact(rawEndTime, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var endTime);
            return endTime;
        }

        private IEnumerator TimerCoroutine()
        {
            var oneSecond = new WaitForSeconds(1);
            TimeSpan deltaTime;
            while (_endTime>DateTime.UtcNow)
            {
                deltaTime=_endTime-DateTime.UtcNow;
                OnRemainingTimeChange?.Invoke($"{deltaTime.Seconds:00}:{deltaTime.Minutes:00}:{deltaTime.Hours:00}");
                yield return oneSecond;
            }
            OnRemainingTimeChange?.Invoke("-");
            _timerCoroutine = default;
        }
    }
}