using System.Collections.Generic;

namespace GameScripts.Game
{
    public interface IShapeCatalog
    {
        public List<ShapeData> AllShapes { get; }
        Dictionary<int, ShapeData> Shapes { get; }
    }
}