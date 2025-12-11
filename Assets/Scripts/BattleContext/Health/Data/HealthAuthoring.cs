using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class HealthAuthoring : MonoBehaviour
{
    public float maxValue;
    public bool showHealthBar;

    class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Health
            {
                currentValue = authoring.maxValue,
                maxValue = authoring.maxValue,
                showHealthBar = authoring.showHealthBar,
            });
        }
    }
}

public struct Health : IComponentData
{
    [GhostField] public float maxValue;
    [GhostField] public float currentValue;
    [GhostField] public bool showHealthBar;
}