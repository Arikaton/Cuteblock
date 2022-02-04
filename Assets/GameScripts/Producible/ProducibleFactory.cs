using System;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.Producible
{
    public class ProducibleFactory : AbstractProducibleFactory
    {
        private IResourceStorage _storage;
        public ProducibleFactory(IResourceStorage storage)
        {
            _storage = storage;
        }
        
        public override IProducible CreateProducible<T>(int count)
        {
            return new Producible<T>(_storage, count);
        }

        public override IProducible CreateProducible(ResourceType resourceType, int count)
        {
            return new Producible(_storage, resourceType, count);
        }
    }
}