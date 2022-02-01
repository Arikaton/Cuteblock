using GameScripts.Game;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameScripts.UI
{
    public abstract class HintViewBase<T> : MonoBehaviour where T : IHintViewModel
    {
        public Button useButton;
        public TextMeshProUGUI quantityText;
        public GameObject plusSign;
        
        private T _hintViewModel;
        private CompositeDisposable _disposables = new CompositeDisposable();

        [Inject]
        public void Construct(T hintViewModel)
        {
            _hintViewModel = hintViewModel;
            useButton.OnClickAsObservable().Subscribe(_ => TryUseHint()).AddTo(_disposables);
            _hintViewModel.Quantity.Subscribe(UpdateQuantity).AddTo(_disposables);
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }

        private void TryUseHint()
        {
            _hintViewModel.TryUse();
        }

        private void UpdateQuantity(int quantity)
        {
            quantityText.text = quantity.ToString();
            plusSign.gameObject.SetActive(quantity == 0);
        }
    }
}