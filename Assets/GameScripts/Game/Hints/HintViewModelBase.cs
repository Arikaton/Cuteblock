using System;
using GameScripts.ConsumeSystem.Module;
using GameScripts.ResourceStorage.Interfaces;
using UniRx;

namespace GameScripts.Game
{
    public abstract class HintViewModelBase<T> : IHintViewModel where T : IResource
    {
        private AbstractConsumableFactory _consumableFactory;
        private IResourceStorage resourceStorage;

        protected FieldViewModelContainer fieldViewModelContainer;
        protected IReactiveProperty<int> quantity;
        
        public IReadOnlyReactiveProperty<int> Quantity { get; }

        protected HintViewModelBase(IResourceStorage resourceStorage, AbstractConsumableFactory consumableFactory, FieldViewModelContainer fieldViewModelContainer)
        {
            this.resourceStorage = resourceStorage;
            _consumableFactory = consumableFactory;
            this.fieldViewModelContainer = fieldViewModelContainer;
            quantity = new ReactiveProperty<int>();
            Quantity = quantity;
            resourceStorage.QuantityChanged += UpdateQuantity;
            quantity.Value = resourceStorage.Quantity<T>();
        }

        private void UpdateQuantity(Type type, int amount)
        {
            quantity.Value = resourceStorage.Quantity<T>();
        }

        public void TryUse()
        {
            if(!CanUse()) return;
            Use();
            // var consumable = _consumableFactory.CreateResourceConsumable<T>(1);
            // if (!consumable.CanConsume())
            //     OpenShopPopup();
            // else
            // {
            //     consumable.Consume();
            //     Use();
            // }
        }

        private void OpenShopPopup()
        {
            
        }

        protected virtual bool CanUse() => true;

        protected abstract void Use();
    }
}