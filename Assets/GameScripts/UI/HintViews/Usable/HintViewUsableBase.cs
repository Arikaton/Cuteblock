using GameScripts.Game;
using UniRx;
using UnityEngine.UI;

namespace GameScripts.UI
{
    public abstract class HintViewUsableBase<T> : HintViewReadOnlyBase<T> where T : IHintViewModel
    {
        public void TryUse()
        {
            viewModel.TryUse();
        }
    }
}