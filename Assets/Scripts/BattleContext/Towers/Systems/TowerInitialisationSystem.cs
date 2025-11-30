using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

public partial struct TowerInitialisationSystem : ISystem
{
    private bool isInit;

    public void OnUpdate(ref SystemState state)
    {
        if (isInit) { return; }
        if (TowerPlacingHelper.Instance == null) { return; }

        // should be taken elsewhere.
        List<PlayerInitialisationInfo> playerInitInfo = new List<PlayerInitialisationInfo>
        {
            new PlayerInitialisationInfo{ kingTowerVisuals = "defaultKingTower", kingTowerLevel = 10, kingTowerIndex = 100, archerTowerVisuals = "defaultArcherTower", archerTowerIndex = 200},
            new PlayerInitialisationInfo{ kingTowerVisuals = "defaultKingTower", kingTowerLevel = 15, kingTowerIndex = 100, archerTowerVisuals = "defaultArcherTower", archerTowerIndex = 200},
        };

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        var buf = SystemAPI.GetSingletonBuffer<TowerLibrary>();

        for(var i = 0; i < playerInitInfo.Count; i++) 
        { 
            var player = playerInitInfo[i];

            CreateTower(ref state, buf, TowerPlacingHelper.TowerType.King, i, player.kingTowerIndex, player.kingTowerVisuals);
            CreateTower(ref state, buf, TowerPlacingHelper.TowerType.LeftArcher, i, player.archerTowerIndex, player.archerTowerVisuals);
            CreateTower(ref state, buf, TowerPlacingHelper.TowerType.RightArcher, i, player.archerTowerIndex, player.archerTowerVisuals);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        isInit = true;
    }

    private void CreateTower(ref SystemState state, 
        DynamicBuffer<TowerLibrary> buf, 
        TowerPlacingHelper.TowerType type, 
        int playerIndex,
        int towerIndex,
        string towerVisuals)
    {
        TowerLibrary concreteSpawnConfig = default;
        foreach (var b in buf)
        {
            if (b.id == towerIndex)
            {
                concreteSpawnConfig = b;
                break;
            }
        }

        var towerPos = TowerPlacingHelper.Instance.GetTowerPos(type, playerIndex + 1);

        // create spawner.
        var entity = state.EntityManager.Instantiate(concreteSpawnConfig.towerSpawnEntity);
        var localTrans = state.EntityManager.GetComponentData<LocalTransform>(entity);
        localTrans.Position = towerPos;
        state.EntityManager.SetComponentData(entity, localTrans);

        var ownerTag = state.EntityManager.GetComponentData<OwnerTag>(entity);
        ownerTag.PlayerId = playerIndex;
        state.EntityManager.SetComponentData(entity, ownerTag);

        // create visuals.
        var visualConfig = type == TowerPlacingHelper.TowerType.King ? TowerConfigLibrary.Instance.GetKingTowerConfig(towerVisuals) : TowerConfigLibrary.Instance.GetArcherTowerConfig(towerVisuals);
        var visuals = TowerPlacingHelper.Instance.AddTowerVisuals(type, playerIndex, visualConfig);

        visuals.GetComponent<EntityToGOLink>().entity = entity;
    }

    public struct PlayerInitialisationInfo
    {
        public int kingTowerLevel;
        public int archerTowerLevel;
        
        public string kingTowerVisuals;
        public string archerTowerVisuals;

        public int kingTowerIndex;
        public int archerTowerIndex;
    }
}
