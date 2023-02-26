using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackState : State
{
    public event UnityAction OnAttackCat;
    public event UnityAction OnHissCat;

    private void OnEnable() 
    {
        Attack();
    }

    private void Attack()
    {
        if(Target.HasSmetana)
        {
            OnAttackCat?.Invoke();
            Debug.Log("Cat Knocked out SMETANA");
        }
        else
        {
            OnHissCat?.Invoke();
            Debug.Log("Cat is Hissing");
        }

    }
}
