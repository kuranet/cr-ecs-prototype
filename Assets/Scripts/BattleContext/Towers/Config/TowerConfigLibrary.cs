using System.Linq;
using UnityEngine;

public class TowerConfigLibrary 
{
    private const string PATH_TO_KING_TOWER_CONFIGS = "SO/KingTowers";
    private const string PATH_TO_ARCHER_TOWER_CONFIGS = "SO/ArcherTowers";

    private static TowerConfigLibrary _instance;
    public static TowerConfigLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TowerConfigLibrary();
            }

            return _instance;
        }
    }

    private TowerConfig[] _kingTowerConfigs;
    private TowerConfig[] _archerTowerConfigs;

    private TowerConfigLibrary()
    {
        _kingTowerConfigs = Resources.LoadAll<TowerConfig>(PATH_TO_KING_TOWER_CONFIGS);
        _archerTowerConfigs = Resources.LoadAll<TowerConfig>(PATH_TO_ARCHER_TOWER_CONFIGS);
    }

    public TowerConfig GetKingTowerConfig(string id)
    {
        return _kingTowerConfigs.FirstOrDefault(config => config._id == id);
    }

    public TowerConfig GetArcherTowerConfig(string id)
    {
        return _archerTowerConfigs.FirstOrDefault(config => config._id == id);
    }
}
