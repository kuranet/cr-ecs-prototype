using Unity.Entities;

public struct DestroyAfterDuration : IComponentData
{
    public float duration;
    public float lifeTime;
}