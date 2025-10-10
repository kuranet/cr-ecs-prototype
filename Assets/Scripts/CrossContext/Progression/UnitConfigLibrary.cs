using System.Linq;
using UnityEngine;

public class UnitConfigLibrary 
{
    private const string PATH_TO_UNIT_CONFIGS = "SO/Units";

    private static UnitConfigLibrary _instance;
    public static UnitConfigLibrary Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UnitConfigLibrary();
            }

            return _instance;
        }
    }

    private UnitConfig[] _unitConfigs;

    private UnitConfigLibrary()
    {
        _unitConfigs = Resources.LoadAll<UnitConfig>(PATH_TO_UNIT_CONFIGS);
    }

    public UnitConfig GetConfig(string id)
    {
        return _unitConfigs.FirstOrDefault(config => config._id == id);
    }
}
