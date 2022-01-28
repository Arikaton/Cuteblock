using System;

namespace GameScripts.ResourceStorage.Interfaces
{
    public interface IResourceStorage
    {
        event Action<Type, int> QuantityChanged;
        int Quantity<T>() where T : IResource;
        int Quantity(Type resourceType);
        void Add<T>(int count) where T : IResource;
        void Add(Type resourceType, int count);
        void Take<T>(int count) where T : IResource;
        void Take(Type resourceType, int count);
        bool CanTake<T>(int count) where T : IResource;
        bool CanTake(Type resourceType, int count);
    }
}