using System.Collections.Generic;
using GameScripts.Game;

namespace GameScripts.Providers
{
    public interface IWeightsProvider
    {
        List<WeightsCatalog.WeightsPreset> Weights { get; }
    }
}