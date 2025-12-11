using Unity.Entities;
using Unity.NetCode;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct GoInGameClientSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach((var networkId, var entity) in SystemAPI.Query<RefRO<NetworkId>>().WithNone<NetworkStreamInGame>().WithEntityAccess())
        {
            ecb.AddComponent<NetworkStreamInGame>(entity);

            var rpcEntity = ecb.CreateEntity();
            ecb.AddComponent(rpcEntity, new GoInGameRpcRequest());
            ecb.AddComponent(rpcEntity, new SendRpcCommandRequest());
        }

        ecb.Playback(state.EntityManager);
    }
}