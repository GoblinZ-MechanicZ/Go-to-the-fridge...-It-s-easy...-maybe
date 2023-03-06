using UnityEngine;

public abstract class EntityAction : ScriptableObject
{
    public abstract void UpdateAction(Entity _entity);
}