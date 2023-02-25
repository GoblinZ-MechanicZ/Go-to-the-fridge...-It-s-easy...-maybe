using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _targetState;
    public State TargetState => _targetState;
    protected Player Target {get; private set;}
    public bool NeedTransit {get; protected set;}

    private void Initialize(Player target)
    {   
        Target = target;
    }

    private void OnEnable() 
    {
        NeedTransit = false;
    }
}
