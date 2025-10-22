using Unity.Entities;

public struct CreationReference : IComponentData
{
    public Entity Creator;
}
