using GameScripts.ResourceStorage.Interfaces;

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
    }
}