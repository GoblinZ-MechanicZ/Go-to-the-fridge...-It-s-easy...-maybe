public abstract class EntityAction
{
    protected Entity entity;

    public void SetupEntityAction(Entity _entity) {
        entity = _entity;
    }

    public abstract void InvokeAction();
}