using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
public partial struct SpawnUnitsClientSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var _singletonQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<BattleState>()
        );

        state.RequireForUpdate(_singletonQuery);
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        if (Input.GetMouseButtonUp(0) && canProcessInput())
        {
            Vector3 worldPoint = CameraManager.GetCameraOrientedPos();

            var rpcEntity = ecb.CreateEntity();
            ecb.AddComponent(rpcEntity, new SpawnCardRpcRequest { 
                localPlayerIndex = LocalPlayer.LocalPlayerIndex, 
                playedCard = 0, 
                worldPoint = worldPoint});

            ecb.AddComponent(rpcEntity, new SendRpcCommandRequest());
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private bool canProcessInput()
    {
        return true;
        return SystemAPI.GetSingleton<BattleState>().isStateValidForPlacingUnits();
    }
}
