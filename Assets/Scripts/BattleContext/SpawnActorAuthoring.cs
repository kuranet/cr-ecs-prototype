using Unity.Entities;
using UnityEngine;

public class SpawnActorAuthoring : MonoBehaviour
{
    public GameObject prefab;
    public GameObject prefab2;

    public class Baker : Baker<SpawnActorAuthoring>
    {
        public override void Bake(SpawnActorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnActorConfig { prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic) });
        }
    }
}

public struct SpawnActorConfig : IComponentData
{
    public Entity prefab;
}
