using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    [SerializeField] private ScreenOrientation _screenOrientation;


    private RectTransform _rectTransform;
    private void Awake()
    {
        if (_screenOrientation != null) _screenOrientation.ScreenOrientationChangeEvent += ResetSafeArea;
        SetSafeArea();
    }

    private void OnDestroy()
    {
        if (_screenOrientation != null) _screenOrientation.ScreenOrientationChangeEvent -= ResetSafeArea;
    }

    private void SetSafeArea() 
    {
        _rectTransform = GetComponent<RectTransform>();
        var safeArea = Screen.safeArea;
        var anchorMin = safeArea.position;
        var anchorMax = anchorMin + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
    }

    public void ResetSafeArea()
    {
        SetSafeArea();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }
}