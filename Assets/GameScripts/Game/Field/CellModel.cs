using UniRx;
using UnityEngine;

namespace GameScripts.Game
{
    [System.Serializable]
    public class CellModel
    {
        public  IReactiveProperty<int> uid;
        public IReactiveProperty<int> hp;
        public Vector2Int positionInShape;
        public Rotation shapeRotation;

        public CellModel()
        {
            uid = new ReactiveProperty<int>(0);
            hp = new ReactiveProperty<int>(0);
        }
    }
}