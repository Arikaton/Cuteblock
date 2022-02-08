using UniRx;

namespace GameScripts.Game
{
    public class FieldModel
    {
        public Flat2DArray<CellModel> FieldMatrix;
        public ShapeModel[] AvailableShapes;
        public IReactiveProperty<int> Score;

        public FieldModel()
        {
            Score = new ReactiveProperty<int>(0);
            AvailableShapes = new ShapeModel[3];
            FieldMatrix = new Flat2DArray<CellModel>(9, 9);
            for (int i = 0; i < 81; i++)
            {
                FieldMatrix.array[i] = new CellModel();
            }
        }

        public FieldModel(Flat2DArray<CellModel> fieldMatrix, ShapeModel[] availableShapes, int score)
        {
            FieldMatrix = fieldMatrix;
            AvailableShapes = availableShapes;
            Score = new ReactiveProperty<int>(score);
        }
    }
}