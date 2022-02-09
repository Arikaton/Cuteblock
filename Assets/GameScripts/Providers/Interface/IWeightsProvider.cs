using System.Collections.Generic;

namespace GameScripts.Providers
{
    public interface IWeightsProvider
    {
        List<int> Weights { get; }
    }
}