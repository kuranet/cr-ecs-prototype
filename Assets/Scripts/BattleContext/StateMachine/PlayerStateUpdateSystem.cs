using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct PlayerStateUpdateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (usingAbilityState, entity) in
                 SystemAPI.Query<UsingAbilityState>()
                 .WithEntityAccess()
                 .WithAll<UnitTag>())
        {
            var abilitySo = usingAbilityState.currentAbility;
            if (usingAbilityState.timeInState > (abilitySo.castDuration + abilitySo.castDelay))
            {
                //UnityEngine.Debug.LogError($"SWITCH TO IDLE");
                ecb.RemoveComponent<UsingAbilityState>(entity);
                ecb.AddComponent(entity, new IdleState());
            }
        }

        foreach (var (localTransform, target, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<Target>>()
                 .WithEntityAccess()
                 .WithAll<UnitTag>()
                 .WithAny<IdleState, MovingState>())
        {
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
                    else if (state.EntityManager.HasComponent<MovingState>(entity))
                    {
                        ecb.RemoveComponent<MovingState>(entity);
                    }

                    //UnityEngine.Debug.LogError($"SWITCH TO ATTACKING STATE");
                }
            }
        }

        foreach (var (localTransform, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>>()
                 .WithEntityAccess()
                 .WithAll<UnitTag>()
                 .WithAll<IdleState>())
        {
            // stay idle because for now we are in range.
            if (UsingAbilityUtil.IsInMovementAbilityRange(entity, state.EntityManager))
            {
                continue;
            }

            // switch from idle to movement
            ecb.RemoveComponent<IdleState>(entity);
            ecb.AddComponent(entity, new MovingState());

            //UnityEngine.Debug.LogError($"SWITCH TO MOVING STATE");
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
