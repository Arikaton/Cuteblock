using GameScripts.ConsumeSystem.Interfaces;
using GameScripts.ResourceStorage.Interfaces;

namespace GameScripts.ConsumeSystem.Module
{
    public class ConsumableFactory : AbstractConsumableFactory
    {
        private IResourceStorage _storage;

        public ConsumableFactory(IResourceStorage storage)
        {
            _storage = storage;
        }
        
        public override IConsumable CreateResourceConsumable<T>(int price)
        {
            return new ResourceConsumable<T>(price, _storage);
        }

        public override IConsumable CreateResourceConsumable<T1, T2>(int price1, int price2)
        {
            return new ResourceConsumable<T1, T2>(price1, price2, _storage);
        }

        public override IConsumable CreateResourceConsumable(PriceTemplate priceTemplate)
        {
            return new ResourceConsumable(priceTemplate, _storage);
        }
    }
}