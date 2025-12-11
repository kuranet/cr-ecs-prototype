using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(TargetSelectionSystem))]
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct UsingAbilityStateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, usingAbilityState, unitEntity) in
                 SystemAPI.Query<RefRO<LocalTransform>, UsingAbilityState>()
                 .WithEntityAccess()
                 .WithAll<CanAttack>())
        {
            usingAbilityState.timeInState += SystemAPI.Time.DeltaTime;

            if (!usingAbilityState.hasCast && usingAbilityState.timeInState > usingAbilityState.currentAbility.castDelay)
            {
                var createdEntity = state.EntityManager.Instantiate(usingAbilityState.currentAbility.prefab);
                var ownerTag = state.EntityManager.GetComponentData<OwnerTag>(unitEntity);
                ecb.AddComponent(createdEntity, new CreationReference() { Creator = unitEntity, playerIndex = ownerTag.PlayerId, canTargetAlly = false, canTargetEnemy = true });

                var abilityLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(createdEntity);
                abilityLocalTransform.ValueRW.Position = transform.ValueRO.Position;

                if (state.EntityManager.HasComponent<AbilityMovement>(createdEntity))
                {
                    var aM = state.EntityManager.GetComponentData<AbilityMovement>(createdEntity);
                    aM.directionToTarget = math.normalize(usingAbilityState.targetPosition - transform.ValueRO.Position);
                    state.EntityManager.SetComponentData(createdEntity, aM);
                }

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
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
