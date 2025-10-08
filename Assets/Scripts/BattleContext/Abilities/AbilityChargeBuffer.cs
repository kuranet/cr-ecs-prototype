using Unity.Entities;

public struct AbilityChargeBuffer : IBufferElementData
{
    public float timeInCooldown;
    public Entity AbilityEntity;
}