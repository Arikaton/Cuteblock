using UniRx;

namespace GameScripts.Game
{
    public class ShapeModel
    {
        public readonly int Uid;
        public IReactiveProperty<Rotation> Rotation;

        public ShapeModel(int uid, Rotation rotation)
        {
            Uid = uid;
            Rotation = new ReactiveProperty<Rotation>(rotation);
        }
    }
}