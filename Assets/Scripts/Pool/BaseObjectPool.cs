using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjectPool<T> : MonoBehaviour where T : IPoolable
{
    [SerializeField] protected T _object;
    protected T _objectPrefab;
    protected List<T> _pooledObjects;
    [SerializeField] protected int _countToPreLoad = 5;

    protected static BaseObjectPool<T> _instance;

    public static BaseObjectPool<T> Instance => _instance;

    protected virtual void Awake() 
    {
        if (_instance == null) _instance = this;
        _pooledObjects = new List<T>();
    }

    public abstract T GetPooledObject();
    //{
    //    for (int i = 0; i < _pooledObjects.Count; i++)
    //    {
    //        var tempT = _pooledObjects[i] as T;
    //        if (tempT == null) continue;

    //        if (!_pooledObjects[i].gameObject.activeInHierarchy)
    //        {
    //            _pooledObjects[i].transform.SetParent(null);
    //            return (T)_pooledObjects[i];
    //        }

    //    }
    //    T o = CreateNewObject<T>();
    //    o.gameObject.transform.SetParent(null);
    //    return o;
    //}

    protected abstract T CreateNewObject();
    //{
    //    var obj = (IPoolable)Instantiate(_objectPrefab.@object, transform.position, Quaternion.identity);

    //    T Tobj = obj as T;
    //    DisableObject(Tobj);
    //    _pooledObjects.Add(Tobj);

    //    return Tobj;
    //}

    public void DisableObject(T obj)
    {
        if (obj == null) return;
        obj.transform.SetParent(transform);
        obj.gameObject.SetActive(false);
    }
}
