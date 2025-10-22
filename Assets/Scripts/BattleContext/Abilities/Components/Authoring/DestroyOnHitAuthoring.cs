using Unity.Entities;
using UnityEngine;

public class DestroyOnHitAuthoring : MonoBehaviour
{
    class Baker : Baker<DestroyOnHitAuthoring>
    {
        public override void Bake(DestroyOnHitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new DestroyOnHit ());
        }
    }
}
