using System.Linq;
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
            foreach (var (identifier, aliveState) in
                     SystemAPI.Query<RefRO<PlayerIdentifier>, RefRW<PlayerAliveState>>())
            {
                if (identifier.ValueRO.playerId != towerOwner.ValueRO.PlayerId) { continue; }

                if (deadTower.ValueRO.type == TowerType.KingTower)
                {
                    SetGameEnd();
                    aliveState.ValueRW.isKingsTowerAlive = false;
                }
                else
                {
                    // get this player king tower and activate it.
                    foreach(var (iden, owner, towerEntity) in SystemAPI.Query<RefRO<TowerIdentifier>, RefRO<OwnerTag>>().WithEntityAccess())
                    {
                        // this is needed king tower.
                        if (iden.ValueRO.type == TowerType.KingTower && owner.ValueRO.PlayerId == towerOwner.ValueRO.PlayerId)
                        {
                            state.EntityManager.SetComponentData(towerEntity, new TowerActiveState { isActive = true});

                            var health = state.EntityManager.GetComponentData<Health>(towerEntity);
                            health.showHealthBar = true;
                            state.EntityManager.SetComponentData(towerEntity, health);
                        }
                    }

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
