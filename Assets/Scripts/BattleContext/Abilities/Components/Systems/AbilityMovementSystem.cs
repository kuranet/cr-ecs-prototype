using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Transforms;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateBefore(typeof(PhysicsSimulationGroup))]
public partial struct AbilityMovementSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, abilityMovement) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<AbilityMovement>>())
        {
            var posBefore = transform.ValueRW.Position;
            transform.ValueRW.Position = transform.ValueRO.Position + abilityMovement.ValueRO.directionToTarget * abilityMovement.ValueRO.movingSpeed * SystemAPI.Time.DeltaTime;
            //var entity = Raycast(posBefore, transform.ValueRW.Position);
            //if (entity != Entity.Null)
            //{
            //    UnityEngine.Debug.LogError($"collide with something");
            //}
        }
    }

    //public Entity Raycast(float3 RayFrom, float3 RayTo)
    //{
    //    // Set up Entity Query to get PhysicsWorldSingleton
    //    // If doing this in SystemBase or ISystem, call GetSingleton<PhysicsWorldSingleton>()/SystemAPI.GetSingleton<PhysicsWorldSingleton>() directly.
    //    EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>();

    //    EntityQuery singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
    //    var collisionWorld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().CollisionWorld;
    //    singletonQuery.Dispose();

    //    RaycastInput input = new RaycastInput()
    //    {
    //        Start = RayFrom,
    //        End = RayTo,
    //        Filter = new CollisionFilter()
    //        {
    //            BelongsTo = ~0u,
    //            CollidesWith = ~0u, // all 1s, so all layers, collide with everything
    //            GroupIndex = 0
    //        }
    //    };

    //    RaycastHit hit = new RaycastHit();
    //    bool haveHit = collisionWorld.CastRay(input, out hit);
    //    if (haveHit)
    //    {
    //        return hit.Entity;
    //    }
    //    return Entity.Null;
    //}
}
