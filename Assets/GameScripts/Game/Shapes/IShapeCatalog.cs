using System.Collections.Generic;

namespace GameScripts.Game
{
    public interface IShapeCatalog
    {
        List<ShapeModel> Shapes { get; }
    }
}