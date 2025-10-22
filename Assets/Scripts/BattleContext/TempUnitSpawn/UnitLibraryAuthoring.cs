using Unity.Entities;
using UnityEngine;

public class UnitLibraryAuthoring : MonoBehaviour
{
    public GameObject[] AbilityPrefabs;

    class Baker : Baker<UnitLibraryAuthoring>
    {
        public override void Bake(UnitLibraryAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            var buffer = AddBuffer<UnitLibrary>(entity);

            foreach (var abilityPrefab in authoring.AbilityPrefabs)
            {
                var abilityEntity = GetEntity(abilityPrefab, TransformUsageFlags.None);
                buffer.Add(new UnitLibrary { unitSpawnEntity = abilityEntity });
            }
        }
    }
}

public struct UnitLibrary : IBufferElementData
{
    public Entity unitSpawnEntity;
}