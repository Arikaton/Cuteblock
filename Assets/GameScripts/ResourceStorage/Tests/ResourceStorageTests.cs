using System;
using System.Collections.Generic;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;
using NUnit.Framework;
using UnityEngine;

namespace GameScripts.ResourceStorage.Tests
{
    public class ResourceStorageTests
    {
        private IResourceStorage _storage;
        private List<Type> _currencyType;
        
        [SetUp]
        public void Setup()
        {
            _currencyType = new List<Type>() {typeof(Gem), typeof(Coin)};
            ClearStorageLocalData();
            _storage = new Module.ResourceStorage(_currencyType);
        }

        [TearDown]
        public void TearDown()
        {
            _storage = null;
            _currencyType = null;
        }

        [Test]
        public void LoadStorage_StorageEmpty()
        {
            Assert.Zero(_storage.Quantity<Gem>());
            Assert.Zero(_storage.Quantity<Coin>());
        }

        [Test]
        public void AddResources_SavedLocally()
        {
            _storage.Add<Gem>(10);
            _storage.Add<Coin>(10);
            _storage = null;
            _storage = new Module.ResourceStorage(_currencyType);
            Assert.AreEqual(10, _storage.Quantity<Gem>());
            Assert.AreEqual(10, _storage.Quantity<Coin>());
        }

        [Test]
        public void AddAbstractResource_ThrowException()
        {
            _currencyType.Add(typeof(IResource));
            Assert.Throws<ArgumentException>(() => _storage = new Module.ResourceStorage(_currencyType));
        }

        [Test]
        public void CanTakeNotPositive_ReturnFalse()
        {
            Assert.Throws<ArgumentException>(() => _storage.CanTake<Gem>(-1));
        }

        [Test]
        public void CanTakeZero_ReturnTrue()
        {
            _storage.Add<Gem>(10);
            _storage.Take<Gem>(10);
            Assert.IsTrue(_storage.CanTake<Gem>(0));
            Assert.IsTrue(_storage.CanTake<Coin>(0));
        }

        [Test]
        public void CanTakeTheSame_ReturnTrue()
        {
            _storage.Add<Gem>(10);
            _storage.Add<Coin>(10);
            
            Assert.IsTrue(_storage.CanTake<Coin>(10));
            Assert.IsTrue(_storage.CanTake<Gem>(10));
        }

        [Test]
        public void CanTakeTwice_ReturnTrue()
        {
            _storage.Add<Coin>(10);
            
            Assert.IsTrue(_storage.CanTake<Coin>(10));
            Assert.IsTrue(_storage.CanTake<Coin>(10));
        }

        [Test]
        public void AddNotExistedCurrency_ThrowException()
        {
            _currencyType.Remove(typeof(Coin));
            _storage = new Module.ResourceStorage(_currencyType);
            Assert.Throws<KeyNotFoundException>(() => _storage.Add<Coin>(10));
        }
        
        [Test]
        public void CanTakeNotExistedCurrency_ThrowException()
        {
            _currencyType.Remove(typeof(Coin));
            _storage = new Module.ResourceStorage(_currencyType);
            Assert.Throws<KeyNotFoundException>(() => _storage.CanTake<Coin>(10));
        }
        
        [Test]
        public void TryTakeNotExistedCurrency_ThrowException()
        {
            _currencyType.Remove(typeof(Coin));
            _storage = new Module.ResourceStorage(_currencyType);
            Assert.Throws<KeyNotFoundException>(() => _storage.Take<Coin>(10));
        }
        
        [Test]
        public void QuantityNotExistedCurrency_ThrowException()
        {
            _currencyType.Remove(typeof(Coin));
            _storage = new Module.ResourceStorage(_currencyType);
            Assert.Throws<KeyNotFoundException>(() => _storage.Quantity<Coin>());
        }

        [Test]
        public void TryTakeMoreThanHave_QuantityNotChanged()
        {
            _storage.Add<Coin>(10);
            _storage.Take<Coin>(100);
            Assert.AreEqual(10, _storage.Quantity<Coin>());
        }

        private void ClearStorageLocalData()
        {
            foreach (var type in _currencyType)
            {
                PlayerPrefs.DeleteKey(Module.ResourceStorage.GetPrefsKey(type));
            }
        }
    }
}
