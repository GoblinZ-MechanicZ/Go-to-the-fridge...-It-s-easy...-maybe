using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FearState : State
{
    [SerializeField] private float _speed;
    [SerializeField] private int _passedPointsCount;
    public int PassedPointsCount => _passedPointsCount;
    public List<Vector3> PassedPoints {get; set;} = new();
    private CatMovement _cat;
    private int _numberOfCurrentPoint;
    public int NumberOfCurrentPoint => _numberOfCurrentPoint;

    private void OnEnable() 
    {
        _cat = GetComponent<CatMovement>();
        _numberOfCurrentPoint = _passedPointsCount - 1;
    }

    private void Update()
    {
        if(_cat.StartMovement == false)
        {
            StartCoroutine(_cat.Move(PassedPoints[_numberOfCurrentPoint], _speed));
            var direction = PassedPoints[_numberOfCurrentPoint] - transform.position;
            _cat.Rotate(direction);
        }
        if(transform.position == PassedPoints[_numberOfCurrentPoint] && _numberOfCurrentPoint >= 1)
        {
            _numberOfCurrentPoint = (_numberOfCurrentPoint - 1) % _passedPointsCount;
        }
    }
}
