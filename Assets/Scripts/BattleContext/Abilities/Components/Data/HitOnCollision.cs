using Unity.Entities;

public struct HitOnCollision : IComponentData
{
    public float radius;
    public float damage;
}