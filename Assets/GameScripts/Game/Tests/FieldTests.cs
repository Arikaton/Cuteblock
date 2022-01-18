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
            _shapeT = new ShapeData(TestId, new Vector2Int(3, 2), (0, 0), (1, 0), (1, 1), (2, 0));
            _fieldModel = new FieldModel();
            var shapeCatalogMock = new Mock<IShapeCatalog>();
            shapeCatalogMock.Setup(x => x.Shapes).Returns(new Dictionary<int, ShapeData> {{_shapeT.Uid, _shapeT}});
            _shapeCatalog = shapeCatalogMock.Object;
            _fieldViewModel = new FieldViewModel(_fieldModel, _shapeCatalog);
        }
    
        [Test]
        public void EmptyField_CanPlaceShape()
        {
            var canPlace = _fieldViewModel.CanPlaceShape(TestId, Rotation.Deg0, new Vector2Int(0,0));
            Assert.IsTrue(canPlace);
        }

        [TearDown]
        public void TearDown()
        {
        
        }
    }
}
