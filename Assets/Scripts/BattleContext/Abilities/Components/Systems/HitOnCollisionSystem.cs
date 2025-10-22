using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct HitOnCollisionSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, hitOnCollision, reference, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<HitOnCollision>, RefRO<CreationReference>>()
                 .WithEntityAccess())
        {
            foreach (var (otherTransform, otherEntity) in
                     SystemAPI.Query<RefRO<LocalTransform>>()
                     .WithAll<Health>()
                     .WithEntityAccess())
            {
                // todo: if target self?
                if (otherEntity == reference.ValueRO.Creator)
                {
                    continue;
                }

                var distanceToOther = math.distance(transform.ValueRO.Position, otherTransform.ValueRO.Position);
                if (distanceToOther < hitOnCollision.ValueRO.radius)
                {
                    var stats = state.EntityManager.GetBuffer<StatsConfig>(entity);
                    var damage = 0f;
                    foreach(var stat in stats)
                    {
                        if(stat.type == StatType.Damage)
                        {
                            damage += stat.addedValue;
                        }
                    }
                    
                    ecb.AddComponent(otherEntity, new AddDamage() { value = damage });
                    ecb.RemoveComponent<HitOnCollision>(entity);

                    if (state.EntityManager.HasComponent<DestroyOnHit>(entity))
                    {
                        ecb.AddComponent(entity, new DestroyAfterDuration() { duration = 0.001f });
                    }

                    break;
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
