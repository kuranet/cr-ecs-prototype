using System.Linq;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial struct ActorSpawnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var query = SystemAPI.QueryBuilder().WithAll<RequestActorSpawn>().Build();
        var spawners = query.ToEntityArray(Allocator.Temp);

        if (AllActorsHolder.Instance == null)
        {
            return;
        }

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var spawner in spawners)
        {
            var warriorGOPrefab = state.EntityManager.GetComponentData<RequestActorSpawn>(spawner);
            var instance = GameObject.Instantiate(warriorGOPrefab.Prefab, AllActorsHolder.Instance.transform);

            var createdEntity = state.EntityManager.Instantiate(warriorGOPrefab.Entity);

            instance.GetComponent<EntityToGOLink>().entity = createdEntity;

            var config = UnitConfigLibrary.Instance.GetConfig(warriorGOPrefab.unitId);
            
            ecb.AddComponent(createdEntity, new Health() { 
                maxValue = config._baseStats.FirstOrDefault(c => c.type == StatType.Health).addedValue,
                currentValue = config._baseStats.FirstOrDefault(c => c.type == StatType.Health).addedValue,
            });

            var buf = ecb.AddBuffer<StatsConfig>(createdEntity);
            foreach (var baseStat in config._baseStats)
            {
                buf.Add(baseStat);
            }

            state.EntityManager.AddComponentObject(createdEntity, instance.GetComponent<Transform>());
            state.EntityManager.AddComponentObject(createdEntity, instance.GetComponent<Animator>());

            state.EntityManager.RemoveComponent<RequestActorSpawn>(spawner);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

