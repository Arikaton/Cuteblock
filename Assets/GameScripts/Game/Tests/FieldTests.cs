using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using UnityEngine;

namespace GameScripts.Game.Tests
{
    public class FieldTests
    {
        private ShapeModel _shapeT;
        private ShapeViewModel _shapeTViewModel;
        private IShapeCatalog _shapeCatalog;
        private FieldModel _fieldModel;
        private FieldViewModel _fieldViewModel;
        private const int TestId = 100;

        [SetUp]
        public void Setup()
        {
            _shapeT = new ShapeModel(new Vector2Int(3, 2), (0, 0), (1, 0), (1, 1), (2, 0));
            _shapeT.uid = TestId;
            _shapeTViewModel = new ShapeViewModel(_shapeT);
            _fieldModel = new FieldModel();
            var shapeCatalogMock = new Mock<IShapeCatalog>();
            shapeCatalogMock.Setup(x => x.Shapes).Returns(new List<ShapeModel> {_shapeT});
            _shapeCatalog = shapeCatalogMock.Object;
            _fieldViewModel = new FieldViewModel(_fieldModel, _shapeCatalog);
        }
    
        [Test]
        public void EmptyField_CanPlaceShape()
        {
            var canPlace = _fieldViewModel.CanPlaceShape(_shapeTViewModel, new Vector2Int(0,0));
            Assert.IsTrue(canPlace);
        }
        
        [Test]
        public void PlaceShape_CellsIdChanges()
        {
            _fieldViewModel.PlaceShape(_shapeTViewModel, new Vector2Int(0, 0));
            foreach (var point in _shapeTViewModel.PointsAfterRotation())
            {
                Assert.AreEqual(TestId, _fieldModel.FieldMatrix[point.x, point.y].uid);
                Assert.AreEqual(point, _fieldModel.FieldMatrix[point.x, point.y].positionInShape);
            }
        }
        
        [Test]
        public void FindBrokenCells_CorrectCellsFound()
        {
            _fieldViewModel.PlaceShape(_shapeTViewModel, new Vector2Int(0, 0));
            _fieldModel.FieldMatrix[0, 0].uid = 0;
            var brokenShapesCells = _fieldViewModel.FindBrokenShapesCells();
            Assert.IsTrue(brokenShapesCells.Contains(new Vector2Int(1,0)));
            Assert.IsTrue(brokenShapesCells.Contains(new Vector2Int(2,0)));
            Assert.IsTrue(brokenShapesCells.Contains(new Vector2Int(1,1)));
            Assert.IsTrue(brokenShapesCells.Count == 3);
        }
    
        [TearDown]
        public void TearDown()
        {
        
        }
    }
}
