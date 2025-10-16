using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct HitOnCollisionSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, hitOnCollision, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<HitOnCollision>>()
                 .WithEntityAccess())
        {
            foreach (var (otherTransform, otherEntity) in
                     SystemAPI.Query<RefRO<LocalTransform>>()
                     .WithAll<Health>()
                     .WithEntityAccess())
            {
                // todo: if target self?
                if (otherEntity == entity)
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

                    break;
                }

            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
