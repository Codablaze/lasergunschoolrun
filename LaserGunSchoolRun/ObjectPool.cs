using System;
using System.Collections.Generic;

namespace LaserGunSchoolRun;

// not tested heavily
public class ObjectPool<T>
    where T : new()
{
    private readonly List<PoolItem> _pool = new();

    public ObjectPool(int capacity)
    {
        for (int i = 0; i < capacity; i++)
            _pool.Add(new PoolItem
            {
                IsAvailable = true,
                Object = new T()
            });
    }

    public T GetFreeObject()
    {
        foreach (var item in _pool)
        {
            if (item.IsAvailable)
            {
                item.IsAvailable = false;
                return item.Object;
            }
        }

        throw new InvalidOperationException($"No free objects in pool. Capacity: {_pool.Count}");
    }

    public void ReleaseObject(T obj)
    {
        foreach (var item in _pool)
        {
            if (item.Object.GetHashCode() == obj.GetHashCode())
                item.IsAvailable = true;
        }
    }

    class PoolItem
    {
        public T Object { get; set; }
        public bool IsAvailable { get; set; }
    }
}
