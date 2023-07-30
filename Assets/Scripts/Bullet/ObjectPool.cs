
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeReference] private Bullet _objectPrefab;
    //[SerializeField] private Bullet _prefab;
    /*[SerializeField] */
    private List<IPoolable> _pooledObjects;
    [SerializeField] private int _countToPreLoad = 5;

    private static ObjectPool _instance;

    public static ObjectPool Instance => _instance;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        _pooledObjects = new List<IPoolable>();
        for (int i = 0; i < _countToPreLoad; i++)
        {
            //CreateNewObject<typeof(_objectPrefab.Type)>();
        }
    }

    public T /*IPoolable*/ GetPooledObject<T>() where T : class, IPoolable
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            var tempT = _pooledObjects[i] as T;
            if (tempT == null) continue;

            if (!_pooledObjects[i].gameObject.activeInHierarchy)
            {
                _pooledObjects[i].transform.SetParent(null);
                return (T)_pooledObjects[i];
            }

        }
        T o = CreateNewObject<T>();
        o.gameObject.transform.SetParent(null);
        return o;
    }

    private T /*IPoolable*/ CreateNewObject<T>() where T : class, IPoolable
    {
        var obj = Instantiate(_objectPrefab, transform.position, Quaternion.identity);

        T Tobj = obj as T;
        DisableObject(Tobj);
        _pooledObjects.Add(Tobj);
        
        return Tobj;
    }

    public void DisableObject(IPoolable obj)
    {
        if (obj == null) return;

        obj.transform.SetParent(transform);
        obj.gameObject.SetActive(false);
    }
}
