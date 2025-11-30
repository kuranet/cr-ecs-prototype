using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSimulationGroup))]
public partial struct HitOnCollisionSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var collisionEvents = SystemAPI.GetSingleton<SimulationSingleton>().AsSimulation().CollisionEvents;

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var collision in collisionEvents)
        {
            if (!GetHitObjectAndHitter(collision, state.EntityManager, out Entity attackEntity, out Entity targetEntity)) { continue; }
            if (!CanAttackerObjectTargetTarget(state.EntityManager, attackEntity, targetEntity)) { continue; }

            var stats = state.EntityManager.GetBuffer<StatsConfig>(attackEntity);
            var damage = 0f;
            foreach (var stat in stats)
            {
                if (stat.type == StatType.Damage)
                {
                    damage += stat.addedValue;
                }
            }

            ecb.AddComponent(targetEntity, new AddDamage() { value = damage });
            ecb.RemoveComponent<HitOnCollision>(attackEntity);

            if (state.EntityManager.HasComponent<DestroyOnHit>(attackEntity))
            {
                ecb.AddComponent(attackEntity, new DestroyAfterDuration() { duration = 0.001f });
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private bool GetHitObjectAndHitter(CollisionEvent e, EntityManager entityManager, out Entity attackEntity, out Entity targetEntity)
    {
        attackEntity = Entity.Null;
        targetEntity = Entity.Null;

        if (entityManager.HasComponent<HitOnCollision>(e.EntityA))
        {
            attackEntity = e.EntityA;
        }
        else if (entityManager.HasComponent<HitOnCollision>(e.EntityB))
        {
            attackEntity = e.EntityB;
        }

        if (entityManager.HasComponent<Health>(e.EntityA))
        {
            targetEntity = e.EntityA;
        }
        else if (entityManager.HasComponent<Health>(e.EntityB))
        {
            targetEntity = e.EntityB;
        }

        return attackEntity != Entity.Null && targetEntity != Entity.Null;
    }

    private bool CanAttackerObjectTargetTarget(EntityManager entityManager, Entity attackEntity, Entity targetEntity)
    {
        var creationRef = entityManager.GetComponentData<CreationReference>(attackEntity);
        var targetEntityOwnerShip = entityManager.GetComponentData<OwnerTag>(targetEntity);

        if (!creationRef.canTargetAlly && creationRef.playerIndex == targetEntityOwnerShip.PlayerId) { return false; }
        if (!creationRef.canTargetEnemy && creationRef.playerIndex != targetEntityOwnerShip.PlayerId) { return false; }

        return true;
    }
}
