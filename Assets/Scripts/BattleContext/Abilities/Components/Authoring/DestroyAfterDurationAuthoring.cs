using Unity.Entities;
using UnityEngine;

public class DestroyAfterDurationAuthoring : MonoBehaviour
{
    public float duration;

    class Baker : Baker<DestroyAfterDurationAuthoring>
    {
        public override void Bake(DestroyAfterDurationAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new DestroyAfterDuration
            {
                duration = authoring.duration,
            });
        }
    }
}