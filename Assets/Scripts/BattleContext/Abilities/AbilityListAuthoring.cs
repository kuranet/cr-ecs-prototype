using Unity.Entities;
using UnityEngine;

public class AbilityListAuthoring : MonoBehaviour
{
    public GameObject[] AbilityPrefabs; 

    class Baker : Baker<AbilityListAuthoring>
    {
        public override void Bake(AbilityListAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            var buffer = AddBuffer<AbilitiesBuffer>(entity);

            foreach (var abilityPrefab in authoring.AbilityPrefabs)
            {
                var abilityEntity = GetEntity(abilityPrefab, TransformUsageFlags.None);
                buffer.Add(new AbilitiesBuffer { AbilityEntity = abilityEntity });
            }
        }
    }
}

public struct AbilitiesBuffer : IBufferElementData
{
    public Entity AbilityEntity;
}