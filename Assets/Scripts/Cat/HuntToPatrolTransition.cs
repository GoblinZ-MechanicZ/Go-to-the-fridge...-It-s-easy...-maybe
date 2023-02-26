using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntToPatrolTransition : Transition
{
    [SerializeField] private float _timeOfStayBeforePatrol;
    private float _expiredTime;
    private void Update() 
    {
        if(GetComponent<CatMovement>().StartMovement == false)
        {
            _expiredTime += Time.deltaTime;
        }

        if(_expiredTime >= _timeOfStayBeforePatrol)
        {
            NeedTransit = true;
            _expiredTime = 0;
        }
    }
}
