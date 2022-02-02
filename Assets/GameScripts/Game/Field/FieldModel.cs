namespace GameScripts.Game
{
    public class FieldModel
    {
        public Flat2DArray<CellModel> FieldMatrix;
        public ShapeModel[] AvailableShapes;
        public int Score;

        public FieldModel()
        {
            AvailableShapes = new ShapeModel[3];
            FieldMatrix = new Flat2DArray<CellModel>(9, 9);
            for (int i = 0; i < 81; i++)
            {
                FieldMatrix.array[i] = new CellModel();
            }
        }
    }
}