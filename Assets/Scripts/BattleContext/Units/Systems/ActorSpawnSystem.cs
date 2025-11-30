using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
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
            var requestInfo = state.EntityManager.GetComponentData<RequestActorSpawn>(spawner);
            var spawnerPosition = requestInfo.requestedPosition;
            var instance = GameObject.Instantiate(requestInfo.Prefab, AllActorsHolder.Instance.transform);

            instance.transform.position = spawnerPosition;

            var createdEntity = state.EntityManager.Instantiate(requestInfo.Entity);

            instance.GetComponent<EntityToGOLink>().entity = createdEntity;

            var ownerTag = state.EntityManager.AddComponentData(createdEntity, new OwnerTag { PlayerId = requestInfo.ownerPlayerId });

            var config = UnitConfigLibrary.Instance.GetConfig(requestInfo.unitId);
            
            ecb.AddComponent(createdEntity, new Health() { 
                maxValue = config._baseStats.FirstOrDefault(c => c.type == StatType.Health).addedValue,
                currentValue = config._baseStats.FirstOrDefault(c => c.type == StatType.Health).addedValue,
            });

            var buf = ecb.AddBuffer<StatsConfig>(createdEntity);
            foreach (var baseStat in config._baseStats)
            {
                buf.Add(baseStat);
            }

            var localTrans = state.EntityManager.GetComponentData<LocalTransform>(createdEntity);
            localTrans.Position = spawnerPosition;
            state.EntityManager.SetComponentData(createdEntity, localTrans);

            state.EntityManager.DestroyEntity(spawner);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

