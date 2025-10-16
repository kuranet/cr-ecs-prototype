using Unity.Entities;
using UnityEngine;

public class RequestActorSpawn : IComponentData
{
    public string unitId;
    public int unitLevel;
    public GameObject Prefab;
    public Entity Entity;
}