using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackToEatTransition : Transition
{
    private void OnEnable() 
    {
        GetComponent<AttackState>().OnAttackCat += Eat;
    }

    private void OnDisable() 
    {
        GetComponent<AttackState>().OnAttackCat -= Eat;
    }

    private void Eat()
    {
        NeedTransit = true;
    }
}
