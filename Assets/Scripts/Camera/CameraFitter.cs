using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFitter : MonoBehaviour
{
    [SerializeField] private Camera _cam;
    [SerializeField] private GameObject _fitObject;
    [SerializeField] private float _spacingFactor = 5;

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space)) SetCamera();
    //}

    public void SetCamera(CinemachineVirtualCamera virtualCamera = null) 
    {
        var bounds = _fitObject.GetComponent<Renderer>().bounds;
        if (_cam.orthographic)
        {
            _cam./*GetPerspectiveFocusTransforms*/GetOrthographicFocusTransforms(out var targetPosition, out var orthographicSize, bounds, _spacingFactor);
            if (virtualCamera)
            {
                var cineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
                cineTransposer.m_FollowOffset = targetPosition;
                virtualCamera.m_Lens.OrthographicSize = orthographicSize;
            }
            else
            {
                _cam.transform.position = targetPosition;
                _cam.orthographicSize = orthographicSize;
            }
        }
        else
        {
            _cam.GetPerspectiveFocusTransforms(out var targetPosition, out var targetRotation, bounds, _spacingFactor);
            if (virtualCamera)
            {
                var cineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
                cineTransposer.m_FollowOffset = targetPosition;
            }
            else _cam.transform.position = targetPosition;
            _cam.transform.rotation = targetRotation;
        }
        

    }

    public (Vector3, float) GetPosAndOrthoSize() 
    {
        var bounds = _fitObject.GetComponent<Renderer>().bounds;
        _cam.GetOrthographicFocusTransforms(out var targetPosition, out var orthographicSize, bounds, _spacingFactor);



        return (targetPosition, orthographicSize);
    }

    public void SetFitObjectPosition(Vector3 pos) 
    {
        _fitObject.transform.position = pos;
    }
}
