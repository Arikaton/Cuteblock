using GameScripts.Game;
using GameScripts.PlayerStats;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.UI.GameOver
{
    public class GameOverView : MonoBehaviour
    {
        private FieldViewModelContainer _fieldViewModelContainer;
        private PlayerStatsViewModel _playerStats;
        private CompositeDisposable _tempDisposables; 
            
        public TextMeshProUGUI currentScoreText;
        public TextMeshProUGUI bestScoreText;

        [Inject]
        public void Construct(FieldViewModelContainer fieldViewModelContainer, PlayerStatsViewModel playerStats)
        {
            _fieldViewModelContainer = fieldViewModelContainer;
            _playerStats = playerStats;
            _fieldViewModelContainer.FieldViewModel.SkipLatestValueOnSubscribe().Subscribe(Initialize).AddTo(this);
            _playerStats.bestScoreEver.Subscribe(value => bestScoreText.text = value.ToString()).AddTo(this);
        }

        private void Initialize(FieldViewModel fieldViewModel)
        {
            _tempDisposables?.Dispose();
            _tempDisposables = new CompositeDisposable();
            _fieldViewModelContainer.FieldViewModel.Value.Score
                .Subscribe(value => currentScoreText.text = value.ToString()).AddTo(_tempDisposables);
        }
    }
}