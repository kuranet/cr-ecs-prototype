using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct CreateVisualsForUnitClientSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach ((var createVisualsRequest, var localTrans, Entity entity) in
            SystemAPI.Query<CreateVisualsForUnit, RefRO<LocalTransform>>().WithEntityAccess())
        {
            var instance = GameObject.Instantiate(createVisualsRequest.Prefab, AllActorsHolder.Instance.transform);

            instance.transform.position = localTrans.ValueRO.Position;
            instance.GetComponent<EntityToGOLink>().entity = entity;

            ecb.RemoveComponent<CreateVisualsForUnit>(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}
