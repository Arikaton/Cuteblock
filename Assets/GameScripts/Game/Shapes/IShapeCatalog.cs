using System.Collections.Generic;

namespace GameScripts.Game
{
    public interface IShapeCatalog
    {
        Dictionary<int, ShapeData> Shapes { get; }
    }
}