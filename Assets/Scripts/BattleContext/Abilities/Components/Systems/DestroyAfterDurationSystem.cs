using Unity.Collections;
using Unity.Entities;

public partial struct DestroyAfterDurationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (destroyAfterDuration, entity) in
                 SystemAPI.Query<RefRW<DestroyAfterDuration>>()
                 .WithEntityAccess())
        {
            destroyAfterDuration.ValueRW.lifeTime += SystemAPI.Time.DeltaTime;

            if (destroyAfterDuration.ValueRO.lifeTime > destroyAfterDuration.ValueRO.duration)
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
