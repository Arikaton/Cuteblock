using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace GameScripts.Game
{
    public class FieldModel
    {
        public Flat2DArray<CellModel> FieldMatrix;
        public ShapeModel[] AvailableShapes;
        public IReactiveProperty<int> Score;
        public IReactiveProperty<int> GemsLeftToCollect;

        public FieldModel(List<Vector2Int> gems)
        {
            Score = new ReactiveProperty<int>(0);
            AvailableShapes = new ShapeModel[3];
            FieldMatrix = new Flat2DArray<CellModel>(9, 9);
            GemsLeftToCollect = new ReactiveProperty<int>(gems.Count);
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    FieldMatrix[x, y] = new CellModel();
                    if (gems.Contains(new Vector2Int(x, y)))
                    {
                        FieldMatrix[x, y].uid.Value = -1;
                        FieldMatrix[x, y].shapeRotation = Rotation.Deg0;
                        FieldMatrix[x, y].positionInShape = new Vector2Int(0, 0);
                    }
                }
            }
        }
    }
}