using UnityEngine;

[CreateAssetMenu(menuName = "Config/Tower")]
public class TowerConfig : ScriptableObject
{
    public string _id;
    public GameObject _prefab;
    public Material _allyMateriel;
    public Material _enemyMaterial;
}
