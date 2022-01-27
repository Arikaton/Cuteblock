using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameScripts.Game
{
    [CreateAssetMenu(fileName = "ShapeModelCatalog", menuName = "Cuteblock/ShapeModelCatalog", order = 0)]
    public class ShapeCatalog : ScriptableObject, IShapeCatalog
    {
        public List<ShapeData> shapes;
        private Dictionary<int, ShapeData> _shapes;

        public List<ShapeData> AllShapes => shapes;

        public Dictionary<int, ShapeData> Shapes
        {
            get
            {
                if (_shapes == null)
                {
                    _shapes = new Dictionary<int, ShapeData>();
                    foreach (var shape in shapes.Where(shape => shape != null)) Shapes.Add(shape.Uid, shape);
                }

                return _shapes;
            }
        }

        private void Awake()
        {
            _shapes = new Dictionary<int, ShapeData>();
            foreach (var shape in shapes)
            {
                _shapes.Add(shape.Uid, shape);
            }
        }
    }
}