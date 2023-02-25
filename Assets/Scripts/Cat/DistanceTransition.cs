using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTransition : Transition
{
    [SerializeField] private float _defaultDistanceToDetection;
    [SerializeField] private float _factorSneakedPlayer;
    [SerializeField] private float _factorChargedPlayer;
    private float _distanceOfDetection;
    private void Update() 
    {
        if(Target.Charged)
            _distanceOfDetection *= _factorChargedPlayer;
        if(Target)
            _distanceOfDetection *= _factorSneakedPlayer;
        if(Target.Charged == false && Target.Sneaked == false)
            _distanceOfDetection = _defaultDistanceToDetection;
        
        if(Vector3.Distance(Target.transform.position, transform.position) <= _distanceOfDetection)
        {
            if(Target.Charged == false && Physics.Raycast(transform.position, Target.transform.position, Vector3.Distance(transform.position, Target.transform.position)) == false)
                NeedTransit = true;
            //TODO: чует сметану через стену
        }
    }
}
