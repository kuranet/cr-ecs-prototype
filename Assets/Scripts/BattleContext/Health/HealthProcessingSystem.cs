using Unity.Collections;
using Unity.Entities;

public partial struct HealthProcessingSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (health, addDamage, entity) in
                 SystemAPI.Query<RefRW<Health>, RefRO<AddDamage>>()
                 .WithEntityAccess())
        {
            health.ValueRW.currentValue -= addDamage.ValueRO.value;
            UnityEngine.Debug.LogError($"apply damage {addDamage.ValueRO.value}");

            ecb.RemoveComponent<AddDamage>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
