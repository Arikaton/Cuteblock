using System;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    public class ResourcesGemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gemsText;
            
        private IResourceStorage _resourceStorage;

        [Inject]
        public void Construct(IResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
        }

        private void Start()
        {
            _resourceStorage.QuantityChanged += ChangeQuantity;
            gemsText.text = _resourceStorage.Quantity<Gem>().ToString();
        }

        private void ChangeQuantity(Type type, int amount)
        {
            gemsText.text = _resourceStorage.Quantity<Gem>().ToString();
        }
    }
}