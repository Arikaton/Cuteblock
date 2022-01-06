namespace GameScripts.Game
{
    public class FieldModel
    {
        public Cell[,] FieldMatrix;

        public FieldModel()
        {
            FieldMatrix = new Cell[9, 9];
        }
    }
}