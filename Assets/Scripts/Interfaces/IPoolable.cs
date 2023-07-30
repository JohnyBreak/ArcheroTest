using UnityEngine;

public interface IPoolable
{
    GameObject gameObject { get; }
    Transform transform { get; }
    public void BackToPool();
}
