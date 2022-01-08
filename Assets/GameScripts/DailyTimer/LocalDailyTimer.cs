using System;
using System.Globalization;
using UniRx;
using UnityEngine;

namespace GameScripts.DailyTimer
{
    public class LocalDailyTimer : IDailyTimer
    {
        private const string _FIRST_SESSION_DAY_PREFS_KEY = "FirstLaunchDayPrefsKey";
        private const string _LAST_SESSION_DAY_PREFS_KEY = "LastSessionDayPrefsKey";
        private const string _GAME_DAYS_COUNT_PREFS_KEY = "GameDaysCountPrefsKey";
        
        private IDateProvider _dateProvider;
        private bool _coldLaunch = true;
        private readonly CompositeDisposable _disposables = new();
        private readonly IReactiveProperty<int> _gameDaysCount;

        public DateTime FirstSessionDay { get; private set; }
        public DateTime LastSessionDay { get; private set; }
        public int DaysAfterLastSession => Mathf.FloorToInt((float)_dateProvider.Today.Date.Subtract(LastSessionDay).TotalDays);
        public bool IsPlayedToday => DaysAfterLastSession == 0;
        public IReadOnlyReactiveProperty<int> SessionsCount { get; }
        public DateTime Today => _dateProvider.Today;


        public LocalDailyTimer(IDateProvider dateProvider)
        {
            _dateProvider = dateProvider;
            FirstSessionDay = LoadFirstSessionDateFromDb();
            
            _gameDaysCount = new ReactiveProperty<int>(PlayerPrefs.GetInt(_GAME_DAYS_COUNT_PREFS_KEY, 0));
            SessionsCount = _gameDaysCount;

            Observable.EveryApplicationFocus().Subscribe(OnApplicationFocus).AddTo(_disposables);
            Observable.OnceApplicationQuit().Subscribe(OnApplicationQuit).AddTo(_disposables);
            Observable.Timer(new TimeSpan(0, 1, 0)).RepeatSafe().Subscribe(OnMinuteLeft).AddTo(_disposables);
            
            UpdateUniqGameDaysCount();
        }

        public void UpdateUniqGameDaysCount()
        {
            LastSessionDay = LoadLastSessionDateFromDb();
            Debug.Log($"Try update uniq game session. Game days count: {SessionsCount.Value}, Is played today: {IsPlayedToday}, " +
                      $"Days after last session: {DaysAfterLastSession}");
            if (IsPlayedToday)
                return;
            _gameDaysCount.Value++;
            PlayerPrefs.SetInt(_GAME_DAYS_COUNT_PREFS_KEY, SessionsCount.Value);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                SaveLastSessionDate();
                _coldLaunch = false;
            }
            else
            {
                if (_coldLaunch)
                    return;
                UpdateUniqGameDaysCount();
            }
        }

        private void OnApplicationQuit(Unit unit)
        {
            SaveLastSessionDate();
            _disposables.Dispose();
        }

        private void OnMinuteLeft(long ticks)
        {
            Debug.Log("Minute left");
            //TODO: Update game sessions
        }

        private void SaveLastSessionDate()
        {
            PlayerPrefs.SetString(_LAST_SESSION_DAY_PREFS_KEY, _dateProvider.Today.ToShortDateString());
        }


        private DateTime LoadFirstSessionDateFromDb()
        {
            DateTime dateTime;
            var stringData = PlayerPrefs.GetString(_FIRST_SESSION_DAY_PREFS_KEY);
            if (string.IsNullOrEmpty(stringData))
            {
                dateTime = _dateProvider.Today;
                PlayerPrefs.SetString(_FIRST_SESSION_DAY_PREFS_KEY, dateTime.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                dateTime = DateTime.Parse(stringData, CultureInfo.InvariantCulture);
            }

            return dateTime;
        }

        private DateTime LoadLastSessionDateFromDb()
        {
            DateTime dateTime;
            var stringData = PlayerPrefs.GetString(_LAST_SESSION_DAY_PREFS_KEY);
            if (string.IsNullOrEmpty(stringData))
            {
                dateTime = DateTime.MinValue;
                PlayerPrefs.SetString(_LAST_SESSION_DAY_PREFS_KEY, dateTime.ToShortDateString());
            }
            else
            {
                dateTime = DateTime.Parse(stringData);
            }

            return dateTime;
        }
    }
}