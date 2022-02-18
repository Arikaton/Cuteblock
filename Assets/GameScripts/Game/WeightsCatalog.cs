using System.Collections.Generic;
using System.Linq;

namespace GameScripts.Game
{
    public class WeightsCatalog
    {
        private List<WeightsPreset> _weightsPresets;

        public WeightsCatalog(List<WeightsPreset> weightsPresets)
        {
            _weightsPresets = weightsPresets;
        }

        public int GetRandomWeightedShapeId(int currentScore)
        {
            var presets = _weightsPresets.Where(x => x.startingScore <= currentScore).OrderByDescending(x => x.startingScore);
            var preset = presets.First();
            return preset.weights.GetRandomWeightedIndex();
        }
        
        [System.Serializable]
        public class WeightsPreset
        {
            public int startingScore;
            public List<int> weights = new List<int>();
            
            
        }
    }
}