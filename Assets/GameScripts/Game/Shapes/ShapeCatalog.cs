using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Game
{
    [CreateAssetMenu(fileName = "ShapeModelCatalog", menuName = "Game/ShapeModelCatalog", order = 0)]
    public class ShapeCatalog : ScriptableObject, IShapeCatalog
    {
        public List<ShapeModel> shapes;

        public List<ShapeModel> Shapes => shapes;
    }
}