using Unity.Entities;
using UnityEngine;

public class AbilityMovementAuthoring : MonoBehaviour
{
    public float movingSpeed;

    class Baker : Baker<AbilityMovementAuthoring>
    {
        public override void Bake(AbilityMovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new AbilityMovement
            {
                movingSpeed = authoring.movingSpeed,
            });
        }
    }
}
