using Unity.Entities;
using Unity.Transforms;

public partial struct AbilityMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, abilityMovement) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<AbilityMovement>>())
        {
            transform.ValueRW.Position = transform.ValueRO.Position + abilityMovement.ValueRO.directionToTarget * abilityMovement.ValueRO.movingSpeed * SystemAPI.Time.DeltaTime;
        }
    }
}
