using Unity.Entities;
using Unity.NetCode;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct SpawnUnitServerSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((var receiveRequest, var requestData, Entity entity) in
            SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, RefRO<SpawnCardRpcRequest>>().WithEntityAccess())
        {
            // process request.
            var buf = SystemAPI.GetSingletonBuffer<UnitLibrary>();
            var ent = state.EntityManager.Instantiate(buf[requestData.ValueRO.playedCard].unitSpawnEntity);

            // set some request info.
            var requestInfo = state.EntityManager.GetComponentObject<RequestActorSpawn>(ent);
            requestInfo.ownerPlayerId = requestData.ValueRO.localPlayerIndex;
            requestInfo.requestedPosition = requestData.ValueRO.worldPoint;

            ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}
