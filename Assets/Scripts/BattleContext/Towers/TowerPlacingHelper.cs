using UnityEngine;

public class TowerPlacingHelper : MonoBehaviour
{
    public enum TowerType { King, LeftArcher, RightArcher };

    [SerializeField] private Transform player1King;
    [SerializeField] private Transform player1ArcherLeft;
    [SerializeField] private Transform player1ArcherRight;

    [SerializeField] private Transform player2King;
    [SerializeField] private Transform player2ArcherLeft;
    [SerializeField] private Transform player2ArcherRight;

    public static TowerPlacingHelper Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetTowerPos(TowerType type, int playerId)
    {
        return GetTransformByType(type, playerId).position;
    }

    public GameObject AddTowerVisuals(TowerType type, int playerId, TowerConfig config)
    {
        var isMyPlayer = playerId == LocalPlayer.LocalPlayerIndex;

        var transformToSpawnUnder = GetTransformByType(type, playerId + 1);

        var visualPrefab = config._prefab;
        var instance = Instantiate(visualPrefab, transformToSpawnUnder);

        var view = instance.GetComponent<TowerVisuals>();
        view.SetMaterial(isMyPlayer ? config._allyMateriel : config._enemyMaterial);

        return instance;
    }

    private Transform GetTransformByType(TowerType type, int playerId)
    {
        if (playerId == 1)
        {
            switch (type)
            {
                case TowerType.King: return player1King;
                case TowerType.LeftArcher: return player1ArcherLeft;
                case TowerType.RightArcher: return player1ArcherRight;
            }
        }

        if (playerId == 2)
        {
            switch (type)
            {
                case TowerType.King: return player2King;
                case TowerType.LeftArcher: return player2ArcherLeft;
                case TowerType.RightArcher: return player2ArcherRight;
            }
        }

        return null;
    }
}
