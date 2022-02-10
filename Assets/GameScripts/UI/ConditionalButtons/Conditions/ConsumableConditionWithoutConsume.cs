using System;
using System.Collections.Generic;
using GameScripts.ConsumeSystem.Module;
using GameScripts.ResourceStorage.ResourceType;
using UnityEngine;
using Zenject;

namespace GameScripts.UI.ConditionalButtons
{
    public class ConsumableConditionWithoutConsume : Condition
    {
        private AbstractConsumableFactory _consumableFactory;
        
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int price;

        [Inject]
        public void Construct(AbstractConsumableFactory consumableFactory)
        {
            _consumableFactory = consumableFactory;
        }
        
        public override void Check(Action<bool> callback)
        {
            var consumable = _consumableFactory.CreateResourceConsumable(new PriceTemplate(new List<PriceData> {new PriceData(price, resourceType)}));
            callback.Invoke(consumable.CanConsume());
        }
    }
}