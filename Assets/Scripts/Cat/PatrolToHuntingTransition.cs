using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatrolToHuntingTransition : Transition
{
    [SerializeField] private float _defaultDistanceToDetection;
    [SerializeField] private float _factorSneakedPlayer;
    [SerializeField] private float _factorChargedPlayer;
    public event UnityAction StartHunting;
    private float _distanceOfDetection;
    private CatMovement _cat;

    private void OnEnable() 
    {
        _cat = GetComponent<CatMovement>();
    }

    private void Update() 
    {
        if(Target.HasSmetana)
            _distanceOfDetection *= _factorChargedPlayer;
        if(Target.IsCrouch)
            _distanceOfDetection *= _factorSneakedPlayer;
        if(Target.HasSmetana == false && Target.IsCrouch == false)
            _distanceOfDetection = _defaultDistanceToDetection;
        
        
        if(Vector3.Distance(Target.transform.position, _cat.Front.position) <= _distanceOfDetection)
        {
            if(Physics.Raycast(_cat.Front.position, Target.transform.position, Vector3.Distance(_cat.Front.position, Target.transform.position), LayerMask.NameToLayer("Player")) == false)
            {
                StartHunting?.Invoke();
                NeedTransit = true;
            }
            //TODO: чует сметану через стену
        }

        _distanceOfDetection = _defaultDistanceToDetection;
    }
}
