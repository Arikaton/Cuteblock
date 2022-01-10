using UniRx;

namespace GameScripts.Game
{
    public class ShapeViewModel
    {
        public int Uid { get; set; }
        public IReactiveProperty<Rotation> Rotation;
        
        private CompositeDisposable _disposable = new CompositeDisposable();

        public ShapeViewModel(int uid, Rotation rotation)
        {
            Uid = uid;
            Rotation = new ReactiveProperty<Rotation>(rotation).AddTo(_disposable);
        }
    }
}