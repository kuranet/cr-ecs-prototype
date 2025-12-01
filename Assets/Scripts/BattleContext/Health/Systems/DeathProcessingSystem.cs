using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

public partial struct DeathProcessingSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        var _singletonQuery = state.GetEntityQuery(
            ComponentType.ReadOnly<BattleState>()
        );

        state.RequireForUpdate(_singletonQuery);
    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (walkabilityBlocker, localTrans) in
                 SystemAPI.Query<RefRO<TileWalkabilityBlocker>, RefRO<LocalTransform>>()
                 .WithAll<DeadTag>()) 
        {
            TileBlockingManager.Instance.
                RemoveBuilding(walkabilityBlocker.ValueRO.blockedLength, walkabilityBlocker.ValueRO.blockedWidth, localTrans.ValueRO.Position);
        }

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
                    SetGameEnd();
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

    private void SetGameEnd()
    {
        var battleState = SystemAPI.GetSingleton<BattleState>();
        battleState.State = BattleStateType.Ended;
        SystemAPI.SetSingleton(battleState);
    }
}
