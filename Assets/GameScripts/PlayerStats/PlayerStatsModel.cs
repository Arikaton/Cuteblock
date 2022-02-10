using UniRx;
using UnityEngine;

namespace GameScripts.PlayerStats
{
    public class PlayerStatsModel
    {
        public IReactiveProperty<int> bestScoreEver;
        public IReactiveProperty<int> bestScoreToday;
        public IReactiveProperty<int> bestScoreWeek;
        public IReactiveProperty<int> bestScoreMonth;
        public IReactiveProperty<int> gamesPlayedThisWeek;
        public IReactiveProperty<int> gamesPlayedThisMonth;

        public PlayerStatsModel()
        {
            bestScoreEver = new ReactiveProperty<int>(0);
            bestScoreToday = new ReactiveProperty<int>(0);
            bestScoreWeek = new ReactiveProperty<int>(0);
            bestScoreMonth = new ReactiveProperty<int>(0);
            gamesPlayedThisWeek = new ReactiveProperty<int>(0);
            gamesPlayedThisMonth = new ReactiveProperty<int>(0);
            
            bestScoreEver.Value = PlayerPrefs.GetInt("BestScoreEver", 0);
            bestScoreEver.Subscribe(value => PlayerPrefs.SetInt("BestScoreEver", value));
        }
    }
}