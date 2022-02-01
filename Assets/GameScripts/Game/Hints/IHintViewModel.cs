using UniRx;

namespace GameScripts.Game
{
    public interface IHintViewModel
    {
        IReadOnlyReactiveProperty<int> Quantity { get; }
        void TryUse();
    }
}