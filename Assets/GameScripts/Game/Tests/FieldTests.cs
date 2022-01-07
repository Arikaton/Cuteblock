using NUnit.Framework;
using UnityEngine;

namespace GameScripts.Game.Tests
{
    public class FieldTests
    {
        private ShapeModel _shapeT;
        private ShapeViewModel _shapeTViewModel;
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
            _fieldViewModel = new FieldViewModel(_fieldModel);
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
                Assert.AreEqual(_fieldModel.FieldMatrix[point.x, point.y].uid, TestId);
                Assert.AreEqual(_fieldModel.FieldMatrix[point.x, point.y].positionInShape, point);
            }
        }
    
        [TearDown]
        public void TearDown()
        {
        
        }
    }
}
