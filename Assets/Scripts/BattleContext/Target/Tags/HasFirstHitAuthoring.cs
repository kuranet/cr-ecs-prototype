using Unity.Entities;
using UnityEngine;

public class HasFirstHitAuthoring : MonoBehaviour
{
    private class Baker : Baker<HasFirstHitAuthoring>
    {
        public override void Bake(HasFirstHitAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HasFirstHitTag());
        }
    }
}

public struct HasFirstHitTag : IComponentData
{
}
