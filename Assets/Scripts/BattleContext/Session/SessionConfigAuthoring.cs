using Unity.Entities;
using UnityEngine;

public class SessionConfigAuthoring : MonoBehaviour
{
    public SessionMode mode;
    public GameObject playerPrefab;

    private class Baker : Baker<SessionConfigAuthoring>
    {
        public override void Bake(SessionConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new SessionConfig
            {
                mode = authoring.mode,
                playerPrefab = GetEntity(authoring.playerPrefab, TransformUsageFlags.None)
            });
        }
    }
}

public struct SessionConfig : IComponentData
{
    public SessionMode mode;
    public Entity playerPrefab;
}
