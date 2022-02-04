using GameScripts.Producible;
using GameScripts.ResourceStorage.ResourceType;
using UnityEngine;
using Zenject;

namespace GameScripts.UI.ConditionalButtons.Actions
{
    public class UseProducibleAction : MonoBehaviour
    { 
        private AbstractProducibleFactory _producibleFactory;

        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int count;

        [Inject]
        public void Construct(AbstractProducibleFactory producibleFactory)
        {
            _producibleFactory = producibleFactory;
        }

        public void UseProducible()
        {
            var producible = _producibleFactory.CreateProducible(resourceType, count);
            producible.Produce();
        }
    }
}