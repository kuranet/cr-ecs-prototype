using Unity.Entities;

public struct CreationReference : IComponentData
{
    public Entity Creator;
    public int playerIndex;
    public bool canTargetAlly;
    public bool canTargetEnemy;
}
