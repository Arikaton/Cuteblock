using UniRx;
using UnityEngine;

namespace GameScripts.Providers
{
    public class CurrentLevelProvider
    {
        private const string Key = "CurrentLevel";

        public ReactiveProperty<int> CurrentLevel;

        public CurrentLevelProvider()
        {
            CurrentLevel = new ReactiveProperty<int>(1);
            CurrentLevel.Value = PlayerPrefs.GetInt(Key, 1);
            CurrentLevel.Subscribe(level => PlayerPrefs.SetInt(Key,level));
        }
    }
}