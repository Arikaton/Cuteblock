using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Providers
{
    [CreateAssetMenu(menuName = "Cuteblock/Weights Catalog", fileName = "WeightsCatalog", order = 0)]
    public class WeightsCatalog : ScriptableObject, IWeightsProvider
    {
        [SerializeField] private List<int> _weights = new List<int>();

        public List<int> Weights => _weights;
    }
}