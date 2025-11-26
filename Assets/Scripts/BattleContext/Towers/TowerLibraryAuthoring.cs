using System;
using Unity.Entities;
using UnityEngine;

public class TowerLibraryAuthoring : MonoBehaviour
{
    //public int id;
    //public GameObject[] TowerPrefabs;
    public TowerConfigAuthoring[] kingTowerList;
    public TowerConfigAuthoring[] archedTowerList;

    class Baker : Baker<TowerLibraryAuthoring>
    {
        public override void Bake(TowerLibraryAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            var buffer = AddBuffer<TowerLibrary>(entity);

            foreach (var towerConfig in authoring.kingTowerList)
            {
                var abilityEntity = GetEntity(towerConfig.towerSpawnEntity, TransformUsageFlags.None);
                buffer.Add(new TowerLibrary { towerSpawnEntity = abilityEntity, id = towerConfig.id });
            }

            foreach (var towerConfig in authoring.archedTowerList)
            {
                var abilityEntity = GetEntity(towerConfig.towerSpawnEntity, TransformUsageFlags.None);
                buffer.Add(new TowerLibrary { towerSpawnEntity = abilityEntity, id = towerConfig.id });
            }
        }
    }
}

[Serializable]
public class TowerConfigAuthoring
{
    public int id;
    public GameObject towerSpawnEntity;
}

[Serializable]
public struct TowerLibrary : IBufferElementData
{
    public int id;
    public Entity towerSpawnEntity;
}