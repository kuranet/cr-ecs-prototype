using Unity.Entities;
using UnityEngine;

public class RequestActorSpawn : IComponentData
{
    public string unitId;
    public int unitLevel;
    public int ownerPlayerId;
    public Vector3 requestedPosition;
    public GameObject Prefab;
    public Entity Entity;
}