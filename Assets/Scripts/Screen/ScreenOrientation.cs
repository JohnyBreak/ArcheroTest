using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenOrientation : MonoBehaviour
{
    public Action ScreenOrientationChangeEvent;

    public enum Orientation 
    {
        Portrait,
        PortraitFixed,
        LandScape,
        LandscapeFixed,
    }

    [SerializeField] private Orientation _orientation;
    
    private UnityEngine.ScreenOrientation _oldOrientation;

    void Awake()
    {
        switch (_orientation)
        {
            case Orientation.Portrait:

                Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
                Screen.orientation = UnityEngine.ScreenOrientation.AutoRotation; 
                Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = true;
                Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = false;
                break;
            case Orientation.PortraitFixed:
                Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
                break;
            case Orientation.LandScape:

                Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
                Screen.orientation = UnityEngine.ScreenOrientation.AutoRotation;
                Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;
                Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
                break;
            case Orientation.LandscapeFixed:
                Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
                break;
        }
        _oldOrientation = Screen.orientation;

        //Destroy(gameObject);
    }
    private void Update()
    {
        if (Screen.orientation == _oldOrientation) return;
        _oldOrientation = Screen.orientation;
        ScreenOrientationChangeEvent?.Invoke();

    }

}
