using Unity.Collections;
using Unity.Entities;

public partial struct InitialiseAbilityChargeBufferSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (abilityBuffer, entity) in
                 SystemAPI.Query<DynamicBuffer<AbilitiesBuffer>>()
                 .WithEntityAccess()
                 .WithAll<UnitTag>()
                 .WithNone<AbilityChargeBuffer>())
        {
            var buffer = ecb.AddBuffer<AbilityChargeBuffer>(entity);

            foreach (var abilityPrefab in abilityBuffer)
            {
                buffer.Add(new AbilityChargeBuffer { AbilityEntity = abilityPrefab.AbilityEntity });
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
