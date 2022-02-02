using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.ResourceStorage.Interfaces
{
    public static class ResourceStorageFactory
    {
        public static IResourceStorage CreateResourceStorage()
        {
            return new Module.ResourceStorage(new[] {typeof(Coin), typeof(Gem), typeof(DeleteHint), typeof(RotateHint), typeof(ReplacementHint)});
        }
    }
}