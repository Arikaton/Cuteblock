using System.Collections.Generic;
using GameScripts.Game;
using UnityEngine;

namespace GameScripts.Providers
{
    [CreateAssetMenu(menuName = "Cuteblock/Weights Catalog", fileName = "WeightsCatalog", order = 0)]
    public class WeightsCatalogue : ScriptableObject, IWeightsProvider
    {
        [SerializeField] private List<Game.WeightsCatalog.WeightsPreset> weightsPresets = new List<Game.WeightsCatalog.WeightsPreset>();

        public List<WeightsCatalog.WeightsPreset> Weights => weightsPresets;
        
        
    }
}