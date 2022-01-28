using System;
using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.ResourceStorage.Module
{
    public static class ResourceFactory
    {
        public static Type FromString(string resourceString)
        {
            return resourceString.ToLower() switch
            {
                "coin" => typeof(Coin),
                "gem" => typeof(Gem),
                _ => throw new ArgumentException($"Resource with type {resourceString} not exist in storage")
            };
        }
    }
}