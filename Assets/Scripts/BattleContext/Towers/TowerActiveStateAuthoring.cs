using Unity.Entities;
using UnityEngine;

public class TowerActiveStateAuthoring : MonoBehaviour
{
    public bool isActive;

    class Baker : Baker<TowerActiveStateAuthoring>
    {
        public override void Bake(TowerActiveStateAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new TowerActiveState
            {
                isActive = authoring.isActive,
            });
        }
    }
}
