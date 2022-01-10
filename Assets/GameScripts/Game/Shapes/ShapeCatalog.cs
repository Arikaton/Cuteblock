using System.Collections.Generic;
using UnityEngine;

namespace GameScripts.Game
{
    [CreateAssetMenu(fileName = "ShapeModelCatalog", menuName = "Game/ShapeModelCatalog", order = 0)]
    public class ShapeCatalog : ScriptableObject, IShapeCatalog
    {
        public List<ShapeData> shapes;
        private Dictionary<int, ShapeData> _shapes;

        public Dictionary<int, ShapeData> Shapes
        {
            get
            {
                if (_shapes == null)
                {
                    _shapes = new Dictionary<int, ShapeData>();
                    foreach (var shape in shapes) Shapes.Add(shape.uid, shape);
                }

                return _shapes;
            }
        }

        private void Awake()
        {
            _shapes = new Dictionary<int, ShapeData>();
            foreach (var shape in shapes)
            {
                _shapes.Add(shape.uid, shape);
            }
        }
    }
}