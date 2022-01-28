using System;
using GameScripts.ConsumeSystem.Interfaces;
using GameScripts.ConsumeSystem.Module;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.Module;
using GameScripts.ResourceStorage.ResourceType;
using Moq;
using NUnit.Framework;

namespace GameScripts.ConsumeSystem.Tests
{
    public class ResourceConsumableTests
    {
        private IConsumable _gemConsumableFullStorage;
        private IConsumable _gemConsumableEmptyStorage;

        private ResourceConsumable<Gem> _emptyGemConsumable;

        private IResourceStorage _fullResourceStorage;
        private IResourceStorage _emptyStorage;
        private ConsumableFactory _consumableFactoryFullStorage;
        private ConsumableFactory _consumableFactoryEmptyStorage;

        private int _startGemsCount = 10;
        private int _startCoinsCount = 10;
        
        [SetUp]
        public void Setup()
        {
            var fullStorageMock = new Mock<IResourceStorage>();
            fullStorageMock.Setup(a => a.CanTake<IResource>(It.IsAny<int>()))
                .Returns(true);
            _fullResourceStorage = fullStorageMock.Object;
            var emptyStorageMock = new Mock<IResourceStorage>();
            emptyStorageMock.Setup(a => a.CanTake<IResource>(It.IsAny<int>()))
                .Returns(false);
            _emptyStorage = emptyStorageMock.Object;
            
            _consumableFactoryFullStorage = new ConsumableFactory(_fullResourceStorage);
            _consumableFactoryEmptyStorage = new ConsumableFactory(_emptyStorage);
            _gemConsumableFullStorage = _consumableFactoryFullStorage.CreateResourceConsumable<Gem>(10);
            _gemConsumableEmptyStorage = _consumableFactoryEmptyStorage.CreateResourceConsumable<Gem>(10);
        }

        [Test]
        public void Consumable_FullStorage_CanConsume()
        {
            Assert.IsTrue(_gemConsumableFullStorage.CanConsume());
        }

        [Test]
        public void Consumable_EmptyStorage_CannotConsume()
        {
            Assert.IsFalse(_gemConsumableEmptyStorage.CanConsume());
        }

        [Test]
        public void ResourceConsumable_Consume_ThemConsumed()
        {
            _gemConsumableFullStorage.Consume();
            Assert.IsTrue(_gemConsumableFullStorage.IsConsumed());
        }

        [Test]
        public void CompositeConsumable_SimilarResources_ThrowException()
        {
            Assert.Throws<ArgumentException>(() =>
                _consumableFactoryFullStorage.CreateResourceConsumable<Gem, Gem>(10, 10));
        }

        [Test]
        public void CompositeConsumable_DifferentResources_DoesntTrow()
        {
            Assert.DoesNotThrow(() => _consumableFactoryFullStorage.CreateResourceConsumable<Gem, Coin>(10, 10));
        }
    }
}
