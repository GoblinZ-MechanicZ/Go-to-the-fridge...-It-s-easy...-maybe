using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackState : State
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distanceToAttack;

    public event UnityAction OnAttackCat;
    public event UnityAction OnHissCat;
    private void Update() 
    {
        var targetPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
        if(Vector3.Distance(transform.position, targetPosition) > _distanceToAttack)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        }
        else
            Attack();

        var direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, angle, 0), 0.01f);
    }

    private void Attack()
    {
        if(Target.Charged)
            OnAttackCat?.Invoke();
        else
            OnHissCat?.Invoke();

    }
}
