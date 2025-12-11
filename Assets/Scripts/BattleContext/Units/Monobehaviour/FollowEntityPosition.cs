using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

public class FollowEntityPosition : MonoBehaviour
{
    public EntityToGOLink linker;

    private void Start()
    {
        if (World.DefaultGameObjectInjectionWorld.IsServer())
        {
            UnityEngine.Debug.LogWarning($"{gameObject.name} has a FollowEntityPosition on server, deactivate it");
            enabled = false;
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        var localTrans = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalTransform>(linker.entity);
        transform.position = localTrans.Position;
    }
}
