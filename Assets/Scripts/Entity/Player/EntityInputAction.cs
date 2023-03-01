using UnityEngine;

public class EntityInputAction : EntityAction
{
    public KeyCode Key;
    public EntityAction action;
    public override void Update(Entity entity)
    {
        if(Input.GetKeyDown(Key)) {
            action.Update(entity);
        }
    }
}