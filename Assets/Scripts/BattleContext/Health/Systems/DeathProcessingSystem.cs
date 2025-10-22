using Unity.Collections;
using Unity.Entities;

public partial struct DeathProcessingSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        // killed towers
        foreach (var (deadTower, towerOwner, entity) in
                 SystemAPI.Query<RefRO<TowerIdentifier>, RefRO<OwnerTag>>()
                 .WithEntityAccess()
                 .WithAll<DeadTag>())
        {
            UnityEngine.Debug.LogError($"tower {towerOwner.ValueRO.PlayerId} {deadTower.ValueRO.type} is dead!");

            foreach (var (identifier, aliveState) in
                     SystemAPI.Query<RefRO<PlayerIdentifier>, RefRW<PlayerAliveState>>())
            {
                if (identifier.ValueRO.playerId != towerOwner.ValueRO.PlayerId) { continue; }

                if (deadTower.ValueRO.type == TowerType.KingTower)
                {
                    aliveState.ValueRW.isKingsTowerAlive = false;
                    UnityEngine.Debug.LogError($"player {identifier.ValueRO.playerId} is dead!");
                }
                else
                {
                    if (aliveState.ValueRO.isLeftTowerAlive)
                    {
                        aliveState.ValueRW.isLeftTowerAlive = false;
                    }
                    else
                    {
                        aliveState.ValueRW.isRightTowerAlive = false;

                    }
                }
            }

            ecb.DestroyEntity(entity);
        }

        foreach (var (unit, entity) in
                 SystemAPI.Query<RefRO<UnitTag>>()
                 .WithEntityAccess()
                 .WithAll<DeadTag>())
        {
            ecb.DestroyEntity(entity);
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
