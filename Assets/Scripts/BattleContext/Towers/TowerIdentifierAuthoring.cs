using Unity.Entities;
using UnityEngine;

public class TowerIdentifierAuthoring : MonoBehaviour
{
    public TowerType type;

    private class Baker : Baker<TowerIdentifierAuthoring>
    {
        public override void Bake(TowerIdentifierAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TowerIdentifier
            {
                type = authoring.type,
            });
        }
    }
}

public struct TowerIdentifier : IComponentData
{
    public TowerType type;
}

public enum TowerType
{
    KingTower, SideTower,
}
