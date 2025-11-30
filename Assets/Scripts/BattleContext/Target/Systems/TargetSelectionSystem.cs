using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct TargetSelectionSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (target, entity) in
                 SystemAPI.Query<RefRW<Target>>()
                 .WithEntityAccess())
        {
            // target is dead. clear it
            if (!state.EntityManager.Exists(target.ValueRO.Object))
            {
                ecb.RemoveComponent<Target>(entity);
            }
        }

        foreach (var (transform, attackTargets, ownerTag, entity) in
              SystemAPI.Query<RefRO<LocalTransform>, RefRO<AttackTargets>, RefRO<OwnerTag>>()
              .WithEntityAccess()
              .WithAll<CanAttack>()
              .WithNone<Target>())
        {
            // if target is dead, clear Target.
            // if has first hit then skip target selection.
            // find closes target to the actor.

            Entity bestTarget = Entity.Null;
            float bestDistSq = float.MaxValue;

            foreach (var (targetTransform, targetEntity) in
                     SystemAPI.Query<RefRO<LocalTransform>>().WithEntityAccess()
                     .WithAll<CanAttack>())
            {
                // Do not target self.
                if (targetEntity == entity)
                {
                    continue;
                }

                // Do not target actors from the same player.
                var targetOwnerTag = state.EntityManager.GetComponentData<OwnerTag>(targetEntity);
                if (targetOwnerTag.PlayerId == ownerTag.ValueRO.PlayerId)
                {
                    continue;
                }

                var type = TargetUtils.GetTargetType(targetEntity, state.EntityManager);
                if ((attackTargets.ValueRO.AllowedTypes & type) == 0)
                    continue;

                float distSq = math.distancesq(transform.ValueRO.Position, targetTransform.ValueRO.Position);
                if (distSq < bestDistSq)
                {
                    bestDistSq = distSq;
                    bestTarget = targetEntity;
                }
            }

            if (bestTarget != Entity.Null)
            {
                if (!state.EntityManager.HasComponent<Target>(entity))
                {
                    ecb.AddComponent(entity, new Target
                    {
                        Object = bestTarget,
                    });
                }
                else
                {
                    var targetComponent = SystemAPI.GetComponentRW<Target>(entity);
                    targetComponent.ValueRW.Object = bestTarget;
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
