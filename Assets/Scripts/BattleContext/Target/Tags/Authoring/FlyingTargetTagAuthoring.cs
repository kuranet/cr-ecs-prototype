using Unity.Entities;
using UnityEngine;

public class FlyingTargetTagAuthoring : MonoBehaviour
{
    private class Baker : Baker<FlyingTargetTagAuthoring>
    {
        public override void Bake(FlyingTargetTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new FlyingTargetTag());
        }
    }
}