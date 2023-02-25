using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFearTransition : Transition
{
    [SerializeField] private float _distanceToEnd;
    private void Update() 
    {
        if(Vector3.Distance(transform.position, Target.transform.position) >= _distanceToEnd)
            NeedTransit = true;
    }
}
