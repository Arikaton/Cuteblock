using UniRx;

namespace GameScripts.Game
{
    public class FieldViewModelContainer
    {
        public IReactiveProperty<FieldViewModel> FieldViewModel;

        public FieldViewModelContainer()
        {
            FieldViewModel = new ReactiveProperty<FieldViewModel>();
        }
    }
}