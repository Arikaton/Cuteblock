using NUnit.Framework;
using UnityEngine;

namespace GameScripts.Game.Tests
{
    public class ShapeTests
    {
        private ShapeData _shape;

        [SetUp]
        public void Setup()
        {
            _shape = new ShapeData(100, new Vector2Int(2, 2), (0, 0), (1, 0), (1, 1));
        }
    
        [Test]
        public void SetRotation_PointsRotatedCorrectly()
        {
            var points = _shape.PointsAfterRotation(Rotation.Deg90);
            Assert.IsTrue(points[0] == new Vector2Int(0,0));
            Assert.IsTrue(points[1] == new Vector2Int(0,1));
            Assert.IsTrue(points[2] == new Vector2Int(-1,1));
        }
    
        [TearDown]
        public void TearDown()
        {
        
        }
    }
}