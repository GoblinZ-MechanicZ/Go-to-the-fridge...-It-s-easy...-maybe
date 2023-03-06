using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EntityRotation", menuName = "Entity/EntityAction/EntityRotation", order = 0)]
public class EntityRotation : EntityAction
{   
    public EntityLookDirection TurnDirection;
    public float TurnAngle = 90;

    public override void UpdateAction(Entity entity)
    {
        
    }
}