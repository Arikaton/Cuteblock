using System;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    public class ResourcesCoinUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;
            
        private IResourceStorage _resourceStorage;

        [Inject]
        public void Construct(IResourceStorage resourceStorage)
        {
            _resourceStorage = resourceStorage;
        }

        private void Start()
        {
            _resourceStorage.QuantityChanged += ChangeQuantity;
            coinsText.text = _resourceStorage.Quantity<Coin>().ToString();
        }

        private void ChangeQuantity(Type type, int amount)
        {
            coinsText.text = _resourceStorage.Quantity<Coin>().ToString();
        }
    }
}
