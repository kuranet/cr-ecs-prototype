using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class TargetUpdatePositionSystem : SystemBase
{
    public event Action<Entity, float3> PositionUpdated;

    protected override void OnUpdate()
    {
        foreach (var (target, navAgentData, entity) in
                 SystemAPI.Query<RefRO<Target>, RefRW<NavAgentData>>().WithEntityAccess())
        {
            var location = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.Object);
            if (float3.Equals(navAgentData.ValueRO.Destination, location.Position) == false)
            {
                navAgentData.ValueRW.Destination = location.Position;
                PositionUpdated?.Invoke(entity, location.Position);
            }
        }
    }
}
