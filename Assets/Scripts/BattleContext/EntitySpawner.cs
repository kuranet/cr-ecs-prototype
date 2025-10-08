using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public partial struct EntitySpawner : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var query = SystemAPI.QueryBuilder().WithAll<ActorGOPrefab>().Build();
        var spawners = query.ToEntityArray(Allocator.Temp);

        if (AllActorsHolder.Instance == null)
        {
            return;
        }

        foreach (var spawner in spawners)
        {
            var warriorGOPrefab = state.EntityManager.GetComponentData<ActorGOPrefab>(spawner);
            var instance = GameObject.Instantiate(warriorGOPrefab.Prefab, AllActorsHolder.Instance.transform);

            var createdEntity = state.EntityManager.Instantiate(warriorGOPrefab.Entity);

            instance.GetComponent<EntityToGOLink>().entity = createdEntity;

            state.EntityManager.AddComponentObject(createdEntity, instance.GetComponent<Transform>());
            state.EntityManager.AddComponentObject(createdEntity, instance.GetComponent<Animator>());
            state.EntityManager.AddComponentData(createdEntity, new ActorGOInstance { Instance = instance });

            state.EntityManager.RemoveComponent<ActorGOPrefab>(spawner);
        }

    }
}

//public event Action<Entity> EntitySpawned;

//protected override void OnCreate()
//{
//    //RequireForUpdate<SpawnActorConfig>();
//}

//protected override void OnUpdate()
//{
//    var config = SystemAPI.GetSingleton<SpawnActorConfig>();

//    if (Input.GetKeyDown(KeyCode.T)) { 
//        var entity = EntityManager.Instantiate(config.prefab);
//        EntitySpawned.Invoke(entity);
//    }
//}
//}
