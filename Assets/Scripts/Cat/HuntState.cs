using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntState : State
{
    [SerializeField] private float _speed;
    private CatMovement _cat;

    private void OnEnable() 
    {
        _cat = GetComponent<CatMovement>();
    }

    private void Update()
    {
        if(_cat.StartMovement == false)
        {
            for(int i = Target.PassedPoints.Count - 1; i >= 0; i--)
            {
                if(Physics.Raycast(_cat.Front.position, new Vector3(Target.PassedPoints[i].x, _cat.Front.position.y, Target.PassedPoints[i].z), 
                Vector3.Distance(_cat.Front.position, Target.PassedPoints[i]), LayerMask.NameToLayer("Player")) == false 
                && _cat.StartMovement == false)
                {
                    _cat.Move(Target.PassedPoints[i], _speed);
                    Debug.Log(i);
                    _cat.Rotate(Target.PassedPoints[i] - transform.position);
                }
            }
        }
    }
}
