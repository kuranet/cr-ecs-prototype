using UnityEngine;

[CreateAssetMenu(menuName = "Config/Unit")]
public class UnitConfig : ScriptableObject
{
    public string _id;
    public string _name;
    public string _description;
    public GameObject _prefab;
    public Sprite _icon;

    public StatsConfig[] _baseStats;
    public UnitLevelConfig[] _levelConfigs;
}
