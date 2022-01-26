using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace GameScripts.Game.Tests
{
    public class FieldTests
    {
        private ShapeData _shapeT;
        private ShapeViewModel _shapeTViewModel;
        private IShapeCatalog _shapeCatalog;
        private FieldModel _fieldModel;
        private FieldViewModel _fieldViewModel;
        private const int TestId = 100;

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void FillRegionWithShapes_OutputIsCorrect()
        {
            var shapes = new List<ShapeData>()
                {new(1, new Vector2Int(1, 1), (0, 0)), new(2, new Vector2Int(2, 1), (0, 0), (1, 0))};
            
            var cells = new HashSet<Vector2Int>
                {new Vector2Int(0, 0), new Vector2Int(1, 1)};

            var output = FieldTools.FillPointsWithShapes(cells, shapes);
        }

        [TearDown]
        public void TearDown()
        {
        
        }
    }
}
