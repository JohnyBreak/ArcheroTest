using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : BaseObjectPool<Coin>
{
    protected override void Awake()
    {
        base.Awake();
        _objectPrefab = _object.GetComponent<Coin>();
    }

    public override Coin GetPooledObject()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].gameObject.activeInHierarchy)
            {
                _pooledObjects[i].transform.SetParent(null);
                return _pooledObjects[i];
            }

        }
        var o = CreateNewObject();
        o.gameObject.transform.SetParent(null);
        return o;
    }

    protected override Coin CreateNewObject()
    {
        var obj = Instantiate(_objectPrefab, transform.position, Quaternion.identity);

        DisableObject(obj);
        _pooledObjects.Add(obj);
        return obj;
    }
}
