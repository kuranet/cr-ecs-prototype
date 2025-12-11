using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class OwnerTagAuthoring : MonoBehaviour
{
    public int playerId;

    private class Baker : Baker<OwnerTagAuthoring>
    {
        public override void Bake(OwnerTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new OwnerTag() { PlayerId = authoring.playerId, });
        }
    }
}

public struct OwnerTag : IComponentData 
{
   [GhostField] public int PlayerId;
}
