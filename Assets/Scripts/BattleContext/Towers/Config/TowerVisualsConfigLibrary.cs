using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/TowerConfigLibrary")]
public class TowerVisualsConfigLibrary : ScriptableObject
{
    [SerializeField] private List<TowerVisualsConfigBundle> configBundles;
    
    public TowerVisualsConfigBundle GetConfigBundle(int identifier)
    {
        return configBundles.FirstOrDefault(b => b.BundleIdentifier == identifier);
    }
}
