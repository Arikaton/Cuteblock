namespace GameScripts.Game
{
    public class FieldModel
    {
        public Flat2DArray<Cell> FieldMatrix;
        public ShapeData[] Shapes;

        public FieldModel()
        {
            Shapes = new ShapeData[3];
            FieldMatrix = new Flat2DArray<Cell>(9, 9);
            for (int i = 0; i < 81; i++)
            {
                FieldMatrix.array[i] = new Cell();
            }
        }
    }
}