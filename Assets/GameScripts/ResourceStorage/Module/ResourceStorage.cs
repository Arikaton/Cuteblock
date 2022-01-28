using System;
using System.Collections.Generic;
using GameScripts.ResourceStorage.Interfaces;
using UnityEngine;

namespace GameScripts.ResourceStorage.Module
{
    internal class ResourceStorage : IResourceStorage
    {
        private readonly IEnumerable<Type> _currencyTypes;
        private Dictionary<Type, int> _storage;

        public event Action<Type, int> QuantityChanged;

        public ResourceStorage(IEnumerable<Type> currencyTypes)
        {
            _currencyTypes = currencyTypes;
            InitializeFromDb();
        }

        public int Quantity<T>() where T : IResource
        {
            return _storage[typeof(T)];
        }

        public int Quantity(Type resourceType)
        {
            return _storage[resourceType];
        }

        public void Add<T>(int count) where T : IResource
        {
            _storage[typeof(T)] += count;
            PlayerPrefs.SetInt(GetPrefsKey(typeof(T)), _storage[typeof(T)]);
            QuantityChanged?.Invoke(typeof(T), count);
        }

        public void Add(Type resourceType, int count)
        {
            _storage[resourceType] += count;
            PlayerPrefs.SetInt(GetPrefsKey(resourceType), _storage[resourceType]);
            QuantityChanged?.Invoke(resourceType, count);
        }

        public void Take<T>(int count) where T : IResource
        {
            if (!CanTake<T>(count))
                return;
            _storage[typeof(T)] -= count;
            PlayerPrefs.SetInt(GetPrefsKey(typeof(T)), _storage[typeof(T)]);
            QuantityChanged?.Invoke(typeof(T), count);
        }

        public void Take(Type resourceType, int count)
        {
            if (!CanTake(resourceType, count))
                return;
            _storage[resourceType] -= count;
            PlayerPrefs.SetInt(GetPrefsKey(resourceType), _storage[resourceType]);
            QuantityChanged?.Invoke(resourceType, count);
        }

        public bool CanTake<T>(int count) where T : IResource
        {
            if (count < 0)
            {
                throw new ArgumentException("Price always must be greater then zero");
            }
            return Quantity<T>() >= count;
        }

        public bool CanTake(Type resourceType, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException("Price always must be greater then zero");
            }

            return Quantity(resourceType) >= count;
        }

        private void InitializeFromDb()
        {
            _storage = new Dictionary<Type, int>();
            foreach (var currencyType in _currencyTypes)
            {
                if (!currencyType.IsClass)
                    throw new ArgumentException("Resource type must be object type");
                var currencyCount = PlayerPrefs.GetInt(GetPrefsKey(currencyType), 0);
                _storage.Add(currencyType, currencyCount);
            }
        }

        internal static string GetPrefsKey(Type type)
        {
            return $"StoragePrefsKey_{type.FullName}";
        }
    }
}