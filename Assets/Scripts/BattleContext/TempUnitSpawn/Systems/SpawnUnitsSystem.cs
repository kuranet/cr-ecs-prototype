using Unity.Entities;
using UnityEngine;

public partial struct SpawnUnitsSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 worldPoint = CameraManager.GetCameraOrientedPos();
            var buf = SystemAPI.GetSingletonBuffer<UnitLibrary>();

            var ent = state.EntityManager.Instantiate(buf[0].unitSpawnEntity);

            // set owner.
            var requestInfo = state.EntityManager.GetComponentObject<RequestActorSpawn>(ent);
            requestInfo.ownerPlayerId = LocalPlayer.LocalPlayerIndex;
            requestInfo.requestedPosition = worldPoint;
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
