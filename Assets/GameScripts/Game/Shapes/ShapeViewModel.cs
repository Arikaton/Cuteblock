using UniRx;

namespace GameScripts.Game
{
    public class ShapeViewModel
    {
        private ShapeModel _model;
        
        public IReadOnlyReactiveProperty<Rotation> Rotation;
        public int Uid => _model.Uid;

        public ShapeViewModel(ShapeModel model)
        {
            _model = model;
            Rotation = _model.Rotation;
        }
    }
}