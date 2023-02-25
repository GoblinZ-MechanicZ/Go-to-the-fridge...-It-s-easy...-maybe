using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    [Header("CatSettings")]
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

        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.forward * 4, 4f) && 
        Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.right * 4, 4f) &&
        Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), -transform.right * 4, 4f) 
        && _cat.StartMovement == false && _cat.StartRotation == false)
            _cat.Rotate(transform.right);
    }

    private void Raycast(Vector3 direction)
    {
        if(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), direction * 4, 4f) == false && _cat.StartMovement == false)
        {
            _currentPoint = transform.position + (direction * 4f);
            if(_fearState.PassedPoints.Count < _fearState.PassedPointsCount)
                _fearState.PassedPoints.Add(_currentPoint);
            else
            {
                _fearState.PassedPoints.RemoveAt(0);
                _fearState.PassedPoints.Add(_currentPoint);
            }
            
            _cat.Rotate(direction);

            StartCoroutine(_cat.Move(_currentPoint));
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), transform.forward * 4);
    }
}
