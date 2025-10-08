using Unity.Entities;
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
    public float maxValue;
    public float currentValue;
    public bool showHealthBar;
}