using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EntityMovement", menuName = "Entity/EntityAction/EntityMovement", order = 0)]
public class EntityMovement : EntityAction
{   
    public EntityLookDirection MovementDirection;
    public float MovementDistance;

    public override void UpdateAction(Entity entity)
    {
        
    }
}