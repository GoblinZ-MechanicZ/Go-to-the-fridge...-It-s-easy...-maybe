using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State _startState;
    private State _currentState;
    private CharacterController _target;

    public State Current => _currentState;
    private void Start()
    {
        _target = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        Reset(_startState);
    }

    private void Update()
    {
        if(_currentState == null)
            return;

        var nextState = _currentState.GetNextState();
        if(nextState != null)
            Transit(nextState);
    }

    private void Transit(State nextState)
    {
        if(_currentState != null)
            _currentState.Exit();

        _currentState = nextState;

        if(_currentState != null)
            _currentState.Enter(_target);
    }

    private void Reset(State startState)
    {
        _currentState = startState;

        if(startState != null)
            _currentState.Enter(_target);
    }
}
