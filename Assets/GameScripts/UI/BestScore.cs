using UniRx;
using GameScripts.PlayerStats;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    public class BestScore : MonoBehaviour
    {
        public TextMeshProUGUI text;
        private PlayerStatsViewModel _viewModel;

        [Inject]
        public void Construct(PlayerStatsViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private void Start()
        {
            text.text = _viewModel.bestScoreEver.Value.ToString();
            _viewModel.bestScoreEver.Subscribe(value => text.text = value.ToString()).AddTo(this);
        }
    }
}