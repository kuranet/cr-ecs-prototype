using Unity.Entities;
using UnityEngine;

public class TileWalkabilityBlockerAuthoring : MonoBehaviour
{
    public OwnerTagAuthoring ownerTagAuthoring;

    public int blockedLength;
    public int blockedWidth;

    class Baker : Baker<TileWalkabilityBlockerAuthoring>
    {
        public override void Bake(TileWalkabilityBlockerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new TileWalkabilityBlocker
            {
                blockedLength = authoring.blockedLength,
                blockedWidth = authoring.blockedWidth,
            });

            if (TileBlockingManager.Instance)
            {
                TileBlockingManager.Instance.AddBuilding(
                    authoring.ownerTagAuthoring.playerId,
                    authoring.blockedLength,
                    authoring.blockedWidth,
                    authoring.transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.Lerp(Color.red, Color.white, 0.3f);
        Gizmos.DrawCube(transform.position, new Vector3(blockedLength, 0.1f, blockedWidth));
    }
}
