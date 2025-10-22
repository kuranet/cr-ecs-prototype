using Unity.Entities;
using UnityEngine;

public class EntityToGOLink : MonoBehaviour
{
    //public int Id;
    public Entity entity;

    private void LateUpdate()
    {
        var isAlive = World.DefaultGameObjectInjectionWorld.EntityManager.Exists(entity);

        if (isAlive)
        {
            return;
        }

        Destroy(gameObject);
    }
}
