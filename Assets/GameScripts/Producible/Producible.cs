using System.Linq;
using GameScripts.ResourceStorage.Interfaces;

namespace GameScripts.Producible
{
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