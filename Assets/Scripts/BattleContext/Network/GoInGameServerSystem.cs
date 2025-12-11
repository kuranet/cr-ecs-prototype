using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct GoInGameServerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((var receiveRequest, Entity entity) in 
            SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRpcRequest>().WithEntityAccess())
        {
            ecb.AddComponent<NetworkStreamInGame>(receiveRequest.ValueRO.SourceConnection);
            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}
