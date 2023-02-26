using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatToPatrolTransition : Transition
{
    [SerializeField] private float _timeForEating;

    private float _expiredTime;
    private void Update() 
    {
        _expiredTime += Time.deltaTime;

        if(_expiredTime >= _timeForEating)
            NeedTransit = true;
    }
}
