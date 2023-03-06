using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EntityInputAction", menuName = "Entity/EntityAction/EntityInputAction", order = 0)]
public class EntityInputAction : EntityAction
{
    public List<InputAction> inputActions = new List<InputAction>();

    public override void UpdateAction(Entity entity)
    {
        foreach(var input in inputActions) {
            if(Input.GetKeyDown(input.Key)) {
                input.Action?.UpdateAction(entity);
            }
        }
    }
}

[System.Serializable]
public struct InputAction {
    public KeyCode Key;
    public EntityAction Action;
}