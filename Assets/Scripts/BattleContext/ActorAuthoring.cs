using Graphical.AnimationWithGameObjects;
using System;
using Unity.Entities;
using UnityEngine;

public class ActorAuthoring : MonoBehaviour
{
    public GameObject Prefab;
    public GameObject Entity;

    class Baker : Baker<ActorAuthoring>
    {
        public override void Bake(ActorAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new ActorGOPrefab
            {
                Prefab = authoring.Prefab,
                Entity = GetEntity(authoring.Entity, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public class ActorGOPrefab : IComponentData
{
    public GameObject Prefab;
    public Entity Entity;
}

public class ActorGOInstance : IComponentData, IDisposable
{
    public GameObject Instance;

    public void Dispose()
    {
        UnityEngine.Object.DestroyImmediate(Instance);
    }
}