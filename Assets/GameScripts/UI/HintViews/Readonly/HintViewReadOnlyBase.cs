using System;
using GameScripts.Game;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace GameScripts.UI
{
    [DisallowMultipleComponent]
    public abstract class HintViewReadOnlyBase<T> : MonoBehaviour where T : IHintViewModel
    {
        public TextMeshProUGUI quantityText;
        public GameObject plusSign;
        
        protected T viewModel;

        [Inject]
        public virtual void Construct(T hintViewModel)
        {
            viewModel = hintViewModel;
            viewModel.Quantity.Subscribe(UpdateQuantity).AddTo(this);
        }

        private void Start()
        {
            plusSign.SetActive(false);
        }

        private void UpdateQuantity(int quantity)
        {
            quantityText.text = quantity.ToString();
            // TODO: uncomment below
            //plusSign.gameObject.SetActive(quantity == 0);
        }
    }
}