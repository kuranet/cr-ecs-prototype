using UnityEngine;

public class TowerVisualsMediator : MonoBehaviour
{
    [SerializeField] private TowerVisualsConfigLibrary _configLibrary;

    [SerializeField] private GameObject _kingTowerRoot;
    [SerializeField] private GameObject _leftArcherTowerRoot;
    [SerializeField] private GameObject _rightArcherTowerRoot;

    public int VisualsType = 0;

    private void Awake()
    {
        var bundle = _configLibrary.GetConfigBundle(VisualsType);

        var kingTowerVisuals = Instantiate(bundle.KingTowerPrefab, _kingTowerRoot.transform);
        var leftArcherTowerVisuals = Instantiate(bundle.ArcherTowerPrefab, _leftArcherTowerRoot.transform);
        var rightArcherTowerVisuals = Instantiate(bundle.ArcherTowerPrefab, _rightArcherTowerRoot.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
