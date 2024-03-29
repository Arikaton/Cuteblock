using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.Producible
{
    public abstract class AbstractProducibleFactory
    {
        public abstract IProducible CreateProducible<T>(int count)  where T : IResource;
        public abstract IProducible CreateProducible(ResourceType resourceType, int count);
    }
}