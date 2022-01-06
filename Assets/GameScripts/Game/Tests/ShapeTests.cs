using NUnit.Framework;

namespace GameScripts.Game.Tests
{
    public class ShapeTests
    {
        private ShapeModel _shape;
        private ShapeViewModel _shapeViewModel;

        [SetUp]
        public void Setup()
        {
            _shape = new ShapeModel(new Vector2IntS(2, 2), (0, 0), (1, 0), (1, 1));
            _shapeViewModel = new ShapeViewModel(_shape);
        }
    
        [Test]
        public void SetRotation_PointsRotatedCorrectly()
        {
            _shapeViewModel.SetRotation(Rotation.Deg90);
            var points = _shapeViewModel.PointsAfterRotation();
            Assert.IsTrue(points[0] == new Vector2IntS(0,0));
            Assert.IsTrue(points[1] == new Vector2IntS(0,1));
            Assert.IsTrue(points[2] == new Vector2IntS(-1,1));
        }
    
        [TearDown]
        public void TearDown()
        {
        
        }
    }
}