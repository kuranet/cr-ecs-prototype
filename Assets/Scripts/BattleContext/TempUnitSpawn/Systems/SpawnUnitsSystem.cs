using Unity.Entities;
using UnityEngine;

public partial struct SpawnUnitsSystem : ISystem
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
            var buf = SystemAPI.GetSingletonBuffer<UnitLibrary>();

            var ent = state.EntityManager.Instantiate(buf[0].unitSpawnEntity);

            // set some request info.
            var requestInfo = state.EntityManager.GetComponentObject<RequestActorSpawn>(ent);
            requestInfo.ownerPlayerId = LocalPlayer.LocalPlayerIndex;
            requestInfo.requestedPosition = worldPoint;
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private bool canProcessInput()
    {
        return SystemAPI.GetSingleton<BattleState>().isStateValidForPlacingUnits();
    }
}
