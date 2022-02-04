using GameScripts.Game;
using UniRx;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public abstract class HintViewUsableBase<T> : HintViewReadOnlyBase<T> where T : IHintViewModel
    {
        public Button useButton;
        
        private void Start()
        {
            useButton.OnClickAsObservable().Subscribe(_ => TryUse()).AddTo(this);
        }
        
        private void TryUse()
        {
            viewModel.TryUse();
        }
    }
}