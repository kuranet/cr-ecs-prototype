using Unity.Entities;

public class TowerSpawningData : IComponentData
{
    public string towerId;
    public int towerLevel;
    public string visualsIdentifier;
    //public GameObject Prefab;
    public Entity Entity;
}
