using UniRx;

namespace GameScripts.PlayerStats
{
    public class PlayerStatsViewModel
    {
        private PlayerStatsModel _model;
        
        public IReadOnlyReactiveProperty<int> bestScoreEver;
        public IReadOnlyReactiveProperty<int> bestScoreToday;
        public IReadOnlyReactiveProperty<int> bestScoreWeek;
        public IReadOnlyReactiveProperty<int> bestScoreMonth;
        public IReadOnlyReactiveProperty<int> gamesPlayedThisWeek;
        public IReadOnlyReactiveProperty<int> gamesPlayedThisMonth;

        public PlayerStatsViewModel(PlayerStatsModel model)
        {
            _model = model;
            bestScoreEver = _model.bestScoreEver;
            bestScoreToday = _model.bestScoreToday;
            bestScoreWeek = _model.bestScoreWeek;
            bestScoreMonth = _model.bestScoreMonth;
            gamesPlayedThisWeek = _model.gamesPlayedThisWeek;
            gamesPlayedThisMonth = _model.gamesPlayedThisMonth;
        }

        public void RecordGameScore(int score)
        {
            if (_model.bestScoreEver.Value < score)
            {
                _model.bestScoreEver.Value = score;
            }
        }
    }
}