using System.Collections.Generic;
using GameScripts.Game;

namespace GameScripts.Providers
{
    public class LevelsProvider
    {
        private List<LevelData> _levels;

        public int LevelsCount => _levels.Count;

        public LevelsProvider(List<LevelData> levels)
        {
            _levels = levels;
        }

        public LevelData GetLevelData(int level)
        {
            return _levels[(level - 1) % _levels.Count];
        }
    }
}