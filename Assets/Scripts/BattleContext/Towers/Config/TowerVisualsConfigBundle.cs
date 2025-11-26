using UnityEngine;

[CreateAssetMenu(menuName = "Config/TowerConfigBundle")]
public class TowerVisualsConfigBundle : ScriptableObject
{
    public int BundleIdentifier;

    public GameObject KingTowerPrefab;
    public GameObject ArcherTowerPrefab;

    public Material allyTowersMaterial;
    public Material enemyTowersMaterial;
}
