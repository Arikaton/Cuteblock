using System;
using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.ConsumeSystem.Module
{
    [Serializable]
    public struct PriceData
    {
        public int price;
        public ResourceType resourceType;
    }
}