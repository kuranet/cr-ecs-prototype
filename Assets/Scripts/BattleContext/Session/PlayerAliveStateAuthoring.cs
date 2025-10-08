using Unity.Entities;
using UnityEngine;

public class PlayerAliveStateAuthoring : MonoBehaviour
{
    private class Baker : Baker<PlayerAliveStateAuthoring>
    {
        public override void Bake(PlayerAliveStateAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerAliveState
            {
                isKingsTowerAlive = true,
                isLeftTowerAlive = true,
                isRightTowerAlive = true,
            });
        }
    }
}

public struct PlayerAliveState : IComponentData
{
    public bool isKingsTowerAlive;
    public bool isLeftTowerAlive;
    public bool isRightTowerAlive;
}
