using Unity.Entities;
using UnityEngine;

public class TowerSpawningAuthoring : MonoBehaviour
{
    public int towerLevel = 1;
    public string towerId;
    public GameObject Entity;

    class Baker : Baker<TowerSpawningAuthoring>
    {
        public override void Bake(TowerSpawningAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new TowerSpawningData
            {
                towerLevel = authoring.towerLevel,
                towerId = authoring.towerId,
                //Prefab = authoring.Prefab,
                Entity = GetEntity(authoring.Entity, TransformUsageFlags.Dynamic),
            }); ;
        }
    }
}
