using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;

// Temp bridge that uses navmesh agent for movement.
public class GoToTarget : MonoBehaviour
{
    public EntityToGOLink linker;
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;

    private void Start()
    {
        // enable obstacle after placing, so player would not be spawned at unawailable position. 
        //var obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }

    private bool skippedFirstTick = false;

    private void Update()
    {
        var isInMovingState = World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<MovingState>(linker.entity);
        agent.enabled = isInMovingState;
        obstacle.enabled = skippedFirstTick && !isInMovingState;

        if (isInMovingState == false)
        {
            // todo: weird, i know.
            if (!skippedFirstTick) { skippedFirstTick = true; }

            return;
        }

        var navData = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<NavAgentData>(linker.entity);
        agent.destination = navData.Destination;

        var localTransform = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(linker.entity);
        localTransform.Position = agent.transform.position;
        World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(linker.entity, localTransform);
    }
}
