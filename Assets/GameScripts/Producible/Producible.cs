using System;
using System.Linq;
using GameScripts.ResourceStorage.Interfaces;
using GameScripts.ResourceStorage.ResourceType;

namespace GameScripts.Producible
{
    public class Producible : IProducible
    {
        private IResourceStorage _resourceStorage;
        private bool _isProduced;
        private int _count;
        private Type _type;

        public Producible(IResourceStorage resourceStorage, ResourceType resourceType, int count)
        {
            _count = count;
            _resourceStorage = resourceStorage;
            _type = Type.GetType($"GameScripts.ResourceStorage.ResourceType.{resourceType.ToString()}, ResourceStorage");
        }

        public void Produce()
        {
            _resourceStorage.Add(_type, _count);
            _isProduced = true;
        }

        public bool IsProduced => _isProduced;
        public string ResourceId => _type.Name;
        public int Amount => _count;
    }
    
    public class Producible<T> : IProducible where T : IResource
    {
        private IResourceStorage _resourceStorage;
        private bool _isProduced;
        private int _count;


        public Producible(IResourceStorage resourceStorage, int count)
        {
            _count = count;
            _resourceStorage = resourceStorage;
        }

        public void Produce()
        {
            _resourceStorage.Add<T>(_count);
            _isProduced = true;
        }

        public bool IsProduced => _isProduced;
        public string ResourceId => typeof(T).ToString().Split('.').Last();
        public int Amount => _count;
    }
}