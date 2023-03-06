using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EntitySanityAction", menuName = "Entity/EntityAction/EntitySanityAction", order = 0)]
public class EntitySanityAction : EntityAction
{
    public float Amount;
    public float MaxAmount;
    public float MinAmount;

    public UnityAction OnSanitiLose;

    public override void UpdateAction(Entity entity)
    {

    }

    public void ChangeSanity(float step) {
        Amount = Mathf.Clamp(Amount + step, MinAmount, MaxAmount);
        if(Amount <= MinAmount) {
            OnSanitiLose?.Invoke();
        }
    }
}