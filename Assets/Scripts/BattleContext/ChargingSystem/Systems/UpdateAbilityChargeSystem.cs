using Unity.Entities;
using Unity.Transforms;

public partial struct UpdateAbilityChargeSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (trans, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>>()
                 .WithEntityAccess()
                 .WithAll<UnitTag>())
        {
            var abilityBuffer = state.EntityManager.GetBuffer<AbilityChargeBuffer>(entity);

            for (var i = 0; i < abilityBuffer.Length; i++)
            {
                var stat = abilityBuffer[i];
                stat.timeInCooldown += SystemAPI.Time.DeltaTime;
                abilityBuffer[i] = stat;
            }
        }
    }
}
