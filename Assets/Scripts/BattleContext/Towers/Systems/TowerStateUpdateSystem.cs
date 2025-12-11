using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateAfter(typeof(TargetSelectionSystem))]
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct TowerStateUpdateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (usingAbilityState, entity) in
                 SystemAPI.Query<UsingAbilityState>()
                 .WithEntityAccess()
                 .WithAll<TowerTag>())
        {
            var abilitySo = usingAbilityState.currentAbility;
            if (usingAbilityState.timeInState > (abilitySo.castDuration + abilitySo.castDelay) || !state.EntityManager.Exists(usingAbilityState.target))
            {
                //UnityEngine.Debug.LogError($"SWITCH TO IDLE");
                ecb.RemoveComponent<UsingAbilityState>(entity);
                ecb.AddComponent(entity, new IdleState());
            }
        }

        foreach (var (localTransform, target, activeState, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<Target>, RefRO<TowerActiveState>>()
                 .WithEntityAccess()
                 .WithAll<TowerTag>()
                 .WithAny<IdleState>())
        {
            // skip inactive towers.
            if (activeState.ValueRO.isActive == false)
            {
                continue;
            }

            var readyAbilities = UsingAbilityUtil.GetUnitAbilitiesBuffer(entity, state.EntityManager);
            if (readyAbilities.Count <= 0)
            {
                continue;
            }

            var targetTransform = state.EntityManager.GetComponentData<LocalTransform>(target.ValueRO.Object);
            var distanceToTarget = math.distance(targetTransform.Position, localTransform.ValueRO.Position);

            foreach (var ability in readyAbilities)
            {
                var abilitySo = state.EntityManager.GetComponentData<Ability>(ability);
                if (distanceToTarget < abilitySo.range)
                {
                    ecb.AddComponent(entity, new UsingAbilityState
                    {
                        currentAbility = abilitySo,
                        target = target.ValueRO.Object,
                        targetPosition = targetTransform.Position,
                    });

                    var chargeBuffer = state.EntityManager.GetBuffer<AbilityChargeBuffer>(entity);

                    for (var i = 0; i < chargeBuffer.Length; i++)
                    {
                        if (chargeBuffer[i].AbilityEntity != ability)
                        {
                            continue;
                        }

                        var stat = chargeBuffer[i];
                        stat.timeInCooldown = 0;
                        chargeBuffer[i] = stat;
                    }

                    if (state.EntityManager.HasComponent<IdleState>(entity))
                    {
                        ecb.RemoveComponent<IdleState>(entity);
                    }

                    //UnityEngine.Debug.LogError($"SWITCH TO ATTACKING STATE");
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
