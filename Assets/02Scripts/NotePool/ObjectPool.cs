using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly Queue<GameObject> pool = new();
    private readonly INoteFactory factory;
    private readonly Transform parent;

    public ObjectPool(INoteFactory factory, Transform parent, int initialSize)
    {
        this.factory = factory;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
            pool.Enqueue(factory.CreateNote(parent));
    }

    public GameObject Get()
    {
        var obj = pool.Count > 0 ? pool.Dequeue() : factory.CreateNote(parent);
        obj.transform.SetParent(parent, false);
        obj.SetActive(true);
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(parent, false);
        pool.Enqueue(obj);
    }
}
