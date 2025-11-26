using Unity.Entities;
using Unity.Transforms;
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

            var localTrans = state.EntityManager.GetComponentData<LocalTransform>(ent);
            localTrans.Position = worldPoint;
            state.EntityManager.SetComponentData(ent, localTrans);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
