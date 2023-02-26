using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    [SerializeField] private float _speed;
    private Vector3 _currentPoint;
    private CatMovement _cat;
    private FearState _fearState;

    private void OnEnable()
    {
        _cat = GetComponent<CatMovement>();
        _fearState = GetComponent<FearState>();

    }

    private void Update() 
    {
        Raycast(transform.forward);
        Raycast(transform.right);
        Raycast(-transform.right);

        if(Physics.Raycast(_cat.Front.position, transform.forward * 4, 4f- (Vector3.Distance(_cat.Front.position, transform.position))) && 
        Physics.Raycast(_cat.Front.position, transform.right * 4, 4f- (Vector3.Distance(_cat.Front.position, transform.position))) &&
        Physics.Raycast(_cat.Front.position, -transform.right * 4, 4f - (Vector3.Distance(_cat.Front.position, transform.position))) 
        && _cat.StartMovement == false && _cat.StartRotation == false)
            _cat.Rotate(transform.right);
    }

    private void Raycast(Vector3 direction)
    {
        if(Physics.Raycast(_cat.Front.position, direction * 4, 4f) == false 
        && _cat.StartMovement == false 
        && _cat.StartRotation == false)
        {
            _currentPoint = transform.position + (direction * 4f);
            if(_fearState.PassedPoints.Count < _fearState.PassedPointsCount)
                _fearState.PassedPoints.Add(_currentPoint);
            else if(_fearState.PassedPointsCount != 0)
            {
                _fearState.PassedPoints.RemoveAt(0);
                _fearState.PassedPoints.Add(_currentPoint);
            }

            var directionForRotation = _currentPoint - transform.position;
            _cat.Rotate(directionForRotation);

            StartCoroutine(_cat.Move(_currentPoint, _speed));
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawRay(_cat.Front.position, transform.forward * 4);
    }
}
