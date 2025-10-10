using Graphical.AnimationWithGameObjects;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEditor.Playables;

public partial struct UsingAbilityStateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, usingAbilityState, unitEntity) in
                 SystemAPI.Query<RefRO<LocalTransform>, UsingAbilityState>()
                 .WithEntityAccess()
                 .WithAll<UnitTag>())
        {
            usingAbilityState.timeInState += SystemAPI.Time.DeltaTime;

            if (!usingAbilityState.hasCast && usingAbilityState.timeInState > usingAbilityState.currentAbility.castDelay)
            {
                var createdEntity = state.EntityManager.Instantiate(usingAbilityState.currentAbility.prefab);

                var localTrans = SystemAPI.GetComponentRW<LocalTransform>(createdEntity);
                localTrans.ValueRW.Position = transform.ValueRO.Position;

                var stats = state.EntityManager.GetBuffer<StatsConfig>(unitEntity);
                var abilityStats = ecb.AddBuffer<StatsConfig>(createdEntity);

                for (var i = 0; i < stats.Length; i++)
                {
                    if (stats[i].type != StatType.Damage)
                    {
                        continue;
                    }

                    abilityStats.Add(stats[i]);
                }

                usingAbilityState.hasCast = true;
                UnityEngine.Debug.LogError($"after {usingAbilityState.timeInState} actually cast ability so");
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
