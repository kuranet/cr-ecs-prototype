using Unity.Entities;
using Unity.Mathematics;

public struct AbilityMovement : IComponentData
{
    public float movingSpeed;
    public float3 directionToTarget;
}
