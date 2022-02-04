using GameScripts.ResourceStorage.Interfaces;

namespace GameScripts.Producible
{
    public abstract class AbstractProducibleFactory
    {
        public abstract IProducible CreateProducible<T>(int count)  where T : IResource;
    }
}