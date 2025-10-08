using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

// Temp bridge that uses navmesh agent for movement.
public class GoToTarget : MonoBehaviour
{
    public EntityToGOLink linker;
    public NavMeshAgent agent;

    private void Awake()
    {
        var system = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<TargetUpdatePositionSystem>();
        system.PositionUpdated += updateDestination;
    }

    private void updateDestination(Entity entity, float3 destination)
    {
        if (entity != linker.entity)
        { 
            UnityEngine.Debug.LogError($"update dest but skip linked {linker} received {entity}");
            return;
        }

        var aa = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<NavAgentData>(entity);
        UnityEngine.Debug.LogError($"update dest {(Vector3)destination}");
        agent.destination = (Vector3)destination;
    }

    private void Update()
    {
        var isInMovingState = World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<MovingState>(linker.entity);
        agent.enabled = isInMovingState;

        if (isInMovingState == false)
        {
            return;
        }

        var localTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(linker.entity);
        localTransform.Position = agent.transform.position;
        World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(linker.entity, localTransform);
    }
}
