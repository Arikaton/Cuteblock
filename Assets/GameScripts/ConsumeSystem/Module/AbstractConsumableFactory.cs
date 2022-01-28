using System;
using System.Collections.Generic;
using GameScripts.ConsumeSystem.Interfaces;
using GameScripts.ResourceStorage.Interfaces;

namespace GameScripts.ConsumeSystem.Module
{
    public abstract class AbstractConsumableFactory
    {
        public abstract IConsumable CreateResourceConsumable<T>(int price) where T : IResource;

        public abstract IConsumable CreateResourceConsumable<T1, T2>(int price1, int price2)
            where T1 : IResource where T2 : IResource;

        public abstract IConsumable CreateResourceConsumable(PriceTemplate priceTemplate);
    }
}