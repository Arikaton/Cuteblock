using NUnit.Framework;

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
            _shapeT = new ShapeModel(new Vector2IntS(3, 2), (0, 0), (1, 0), (1, 1), (2, 0));
            _shapeT.uid = TestId;
            _shapeTViewModel = new ShapeViewModel(_shapeT);
            _fieldModel = new FieldModel();
            _fieldViewModel = new FieldViewModel(_fieldModel);
        }
    
        [Test]
        public void EmptyField_CanPlaceShape()
        {
            var canPlace = _fieldViewModel.CanPlaceShape(_shapeTViewModel, new Vector2IntS(0,0));
            Assert.IsTrue(canPlace);
        }
    
        [TearDown]
        public void TearDown()
        {
        
        }
    }
}
