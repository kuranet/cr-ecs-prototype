using Unity.Entities;
using UnityEngine;

public class HitOnCollisionAuthoring : MonoBehaviour
{
    public float radius;
    public float damage;

    class Baker : Baker<HitOnCollisionAuthoring>
    {
        public override void Bake(HitOnCollisionAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new HitOnCollision
            {
                radius = authoring.radius,
                damage = authoring.damage,
            });
        }
    }
}
