using GameScripts.Providers;
using TMPro;
using UnityEngine;
using Zenject;
using UniRx;

namespace GameScripts.UI
{
    public class CurrentLevelText : MonoBehaviour
    {
        public TextMeshProUGUI text;
        private CurrentLevelProvider _currentLevelProvider;

        [Inject]
        public void Construct(CurrentLevelProvider currentLevelProvider, LevelsProvider levelsProvider)
        {
            currentLevelProvider.CurrentLevel.Subscribe(level => text.text = $"LEVEL {((level-1) % levelsProvider.LevelsCount + 1).ToString()}");
        }
    }
}