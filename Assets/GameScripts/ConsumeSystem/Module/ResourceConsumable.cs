using System;
using System.Collections.Generic;
using System.Linq;
using GameScripts.ConsumeSystem.Interfaces;
using GameScripts.ResourceStorage.Interfaces;

namespace GameScripts.ConsumeSystem.Module
{
    public class ResourceConsumable : IConsumable
    {
        private IResourceStorage _resourceStorage;
        private readonly Dictionary<Type, int> resourceToPrice;
        private bool _consumed = false;
        public ResourceConsumable(PriceTemplate priceTemplate, IResourceStorage resourceStorage)
        {
            resourceToPrice = new Dictionary<Type, int>(priceTemplate.prices.Count);
            foreach (var price in priceTemplate.prices)
            {
                var resourceType = Type.GetType($"GameScripts.ResourceStorage.ResourceType.{price.resourceType.ToString()}, ResourceStorage");
                resourceToPrice.Add(resourceType, price.price);
            }
            _resourceStorage = resourceStorage;
        }
        
        public bool CanConsume()
        {
            return resourceToPrice.All(resourceData => _resourceStorage.CanTake(resourceData.Key, resourceData.Value));
        }

        public void Consume()
        {
            foreach (var resourceData in resourceToPrice)
            {
                _resourceStorage.Take(resourceData.Key, resourceData.Value);
            }

            _consumed = true;
        }

        public bool IsConsumed()
        {
            return _consumed;
        }
    }
    
    public class ResourceConsumable<T> : IConsumable where T : IResource
    {
        private readonly int _price;
        private readonly IResourceStorage _resourceStorage;
        private bool _isConsumed;

        internal ResourceConsumable(int price, IResourceStorage resourceStorage)
        {
            _price = price;
            _resourceStorage = resourceStorage;
        }

        public bool CanConsume()
        {
            return _resourceStorage.CanTake<T>(_price);
        }

        public void Consume()
        {
            if (!CanConsume())
                return;
            _isConsumed = true;
            _resourceStorage.Take<T>(_price);
        }

        public bool IsConsumed()
        {
            return _isConsumed;
        }
    }

    public class ResourceConsumable<T1, T2> : IConsumable 
        where T1 : IResource where T2 : IResource
    {
        private readonly int _price1;
        private readonly int _price2;
        private readonly IResourceStorage _resourceStorage;
        private bool _isConsumed;

        public ResourceConsumable(int price1, int price2, IResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
            _price2 = price2;
            _price1 = price1;
            if (typeof(T1) == typeof(T2))
                throw new ArgumentException($"Resource type must be different, but they both '{typeof(T1)}'");
        }
        
        public bool CanConsume()
        {
            var canConsume1 = _resourceStorage.CanTake<T1>(_price1);
            var canConsume2 = _resourceStorage.CanTake<T2>(_price2);
            return canConsume1 && canConsume2;
        }

        public void Consume()
        {
            if (!CanConsume())
                return;
            _resourceStorage.Take<T1>(_price1);
            _resourceStorage.Take<T2>(_price2);
            _isConsumed = true;
        }

        public bool IsConsumed()
        {
            return _isConsumed;
        }
    }
}