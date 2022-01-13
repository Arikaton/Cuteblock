using UniRx;

namespace GameScripts.Game
{
    public class ShapeViewModelsContainer
    {
        public ReactiveCommand<int> viewModelChanged;
        public ShapeViewModel[] shapeViewModels;

        public ShapeViewModelsContainer()
        {
            viewModelChanged = new ReactiveCommand<int>();
            shapeViewModels = new ShapeViewModel[3];
        }

        public void ChangeViewModel(int index, ShapeViewModel shapeViewModel)
        {
            shapeViewModels[index] = shapeViewModel;
            viewModelChanged.Execute(index);
        }
    }
}