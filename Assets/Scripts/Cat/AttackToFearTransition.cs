using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToFearTransition : Transition
{
    [SerializeField] private float _delayBeforeFear;
    private float _expiredTime;
    private void Update() 
    {
        _expiredTime += Time.deltaTime;
        if(_expiredTime >= _delayBeforeFear)
            NeedTransit = true;
    }
}
