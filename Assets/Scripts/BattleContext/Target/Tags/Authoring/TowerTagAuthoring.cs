using Unity.Entities;
using UnityEngine;

public class TowerTagAuthoring : MonoBehaviour
{
    private class Baker : Baker<TowerTagAuthoring>
    {
        public override void Bake(TowerTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TowerTag());
            AddComponent(entity, new CanAttack());
            AddComponent(entity, new IdleState());

            var buffer = AddBuffer<StatsConfig>(entity);
            buffer.Add(new StatsConfig() { type = StatType.Damage, addedValue = 50 });
        }
    }
}
