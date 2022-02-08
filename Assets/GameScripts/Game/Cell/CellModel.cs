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

        public CellModel(int uid, int hp, Vector2Int positionInShape, Rotation shapeRotation)
        {
            this.uid = new ReactiveProperty<int>(uid);
            this.hp = new ReactiveProperty<int>(hp);
            this.positionInShape = positionInShape;
            this.shapeRotation = shapeRotation;
        }
    }
}