using Unity.Entities;
using UnityEngine;

public class AbilityAuthoring : MonoBehaviour
{
    public float range;
    public float cooldown;
    public float castDelay;
    public float castDuration;
    public GameObject prefab;

    class Baker : Baker<AbilityAuthoring>
    {
        public override void Bake(AbilityAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new Ability
            {
                range = authoring.range,
                cooldown = authoring.cooldown,
                castDelay = authoring.castDelay,
                castDuration = authoring.castDuration,
                prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic)
            });
        }
    }
}
