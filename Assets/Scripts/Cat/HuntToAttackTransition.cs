using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntToAttackTransition : Transition
{
    [SerializeField] private float _distanceToAttack;

    private CatMovement _cat;

    private void OnEnable() 
    {
        _cat = GetComponent<CatMovement>();
    }

    private void Update() 
    {
        if(Vector3.Distance(transform.position, Target.transform.position) <= _distanceToAttack
        && Physics.Raycast(_cat.Front.position, Target.transform.position, Vector3.Distance(_cat.Front.position, Target.transform.position), LayerMask.NameToLayer("Player")) == false)
        {
            _cat.StopAllCoroutines();
            NeedTransit = true;  
        }  
    }
}
