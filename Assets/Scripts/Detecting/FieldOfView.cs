using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public Action<Transform> TargetFindEvent;
    public Action<Vector3> TargetLostEvent;

    [SerializeField, Range(0, 360)] private float _viewAngle = 90;
    [SerializeField] private float _viewRadius = 7;
    //[SerializeField] private float _stopDistance;
    [SerializeField] private float _offsetY = 0.7f;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstacleMask;

    private List<Target> _targetsList = new List<Target>();
    private List<Target> _visibleTargets = new List<Target>();
    private Coroutine _findTargetsRoutine;
    private Target _currentTarget;
    //private Vector3 _lastTargetPos;
    private float _minDist;

    public Target CurrentTarget => _currentTarget;

    private void Start()
    {
        StartFindTarget();
    }
    
    private void OnDisable()
    {
        StopFindTarget();
    }

    public void StartFindTarget()
    {
        _findTargetsRoutine = StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    public void StopFindTarget()
    {
        if (_findTargetsRoutine != null)
        {
            ClearVariables();
            StopCoroutine(_findTargetsRoutine);
            _findTargetsRoutine = null;
        }
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    //private void Update()
    //{
    //    //_position = new Vector3(transform.position.x, transform.position.y + _offsetY, transform.position.z);
    //}

    public void SetViewAngle(float angle)
    {
        _viewAngle = angle;
    }

    public void SetViewRadius(float radius)
    {
        _viewRadius = radius;
    }

    private void ClearVariables() 
    {
        _minDist = float.MaxValue;
        _currentTarget = null;
        _targetsList.Clear();
        _visibleTargets.Clear();
    } 

    private void FindVisibleTargets()
    {
        ClearVariables();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

        //if (targetsInViewRadius.Length > 0) 
        //{
        //    targetsInViewRadius[0].TryGetComponent<Target>(out var player);
        //    _currentTarget = player;
        //}


        foreach (var item in targetsInViewRadius)
        {
            if (item.TryGetComponent<Target>(out var enemy))
            {
                _targetsList.Add(enemy);
                //_currentTarget = enemy;
            }
        }

        foreach (var item in _targetsList)
        {
            //if (_currentTarget != null)
            //{
                Vector3 dirToTarget = (item/*_currentTarget*/.transform.position - transform.position + transform.up * _offsetY).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < _viewAngle / 2)
                {
                    //float distTotarget = CheckDistanceToTarget(item.transform);//Vector3.Distance(transform.position + transform.up * _offsetY, item/*_currentTarget*/.transform.position);
                    if (!Physics.Raycast(transform.position + transform.up * _offsetY, dirToTarget, _viewRadius + 10/*distTotarget*/, _obstacleMask))
                    {
                        _visibleTargets.Add(item); //_currentTarget);
                                                   //item.LogDetection();
                                                   //_currentTarget = _visibleTargets[0];
                    //if (_minDist > distTotarget) 
                    //{
                    //    _minDist = distTotarget;
                    //    _currentTarget = item;
                    //}
                        //_lastTargetPos = _currentTarget.transform.position;
                        //TargetFindEvent?.Invoke(_currentTarget.transform);
                        //_navAgent.SetDestination(_currentTarget.transform.position);
                    }
                    //else
                    //{
                    //    Lost();
                    //    //_navAgent.SetDestination(_lastTargetPos);
                    //}
                    //if (distTotarget > _viewRadius) Lost();
                }
            //}
        }
        foreach (var item in _visibleTargets)
        {
            float distTotarget = CheckDistanceToTarget(item.transform);
            if (_minDist > distTotarget)
            {
                _minDist = distTotarget;
                _currentTarget = item;
            }
        }
    }

    //private void Lost() 
    //{
    //    TargetLostEvent?.Invoke(_lastTargetPos);
    //    _currentTarget = null;
    //}
    private float CheckDistanceToTarget(Transform target)
    {
        float distance = (((target.position.x - transform.position.x) * (target.position.x - transform.position.x)) +
            ((target.position.y - transform.position.y) * (target.position.y - transform.position.y)) +
            ((target.position.z - transform.position.z) * (target.position.z - transform.position.z)));
        return distance;
    }
    //private bool CheckTargetDistance()
    //{
    //    _stopDistance = (((_lastTargetPos.x - transform.position.x) * (_lastTargetPos.x - transform.position.x)) +
    //        ((_lastTargetPos.y - transform.position.y) * (_lastTargetPos.y - transform.position.y)) +
    //        ((_lastTargetPos.z - transform.position.z) * (_lastTargetPos.z - transform.position.z)));
    //    Debug.Log(_stopDistance);
    //    if (_stopDistance > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}


#if UNITY_EDITOR
    public Vector3 DirectionFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        angleDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireArc(transform.position + transform.up * _offsetY, Vector3.up, Vector3.forward, 360, _viewRadius);
        Vector3 viewAngleA = DirectionFromAngle(-_viewAngle / 2, false);
        Vector3 viewAngleB = DirectionFromAngle(_viewAngle / 2, false);

        Handles.DrawLine((transform.position + transform.up * _offsetY), (transform.position + transform.up * _offsetY) + viewAngleA * _viewRadius);
        Handles.DrawLine((transform.position + transform.up * _offsetY), (transform.position + transform.up * _offsetY) + viewAngleB * _viewRadius);

        Handles.color = Color.yellow;
        foreach (Target item in _targetsList)
        {
            if (item == null) continue;
            Handles.DrawLine(transform.position + transform.up * _offsetY, item.transform.position);
        }

        Handles.color = Color.red;
        foreach (Target item in _visibleTargets)
        {
            if (item == null) continue;
            Handles.DrawLine(transform.position + transform.up * _offsetY, item.transform.position);
        }

        if (_currentTarget == null) return;

        Handles.color = Color.blue;
        Handles.DrawLine(transform.position + transform.up * _offsetY, _currentTarget.transform.position);
        
    }
#endif
}
