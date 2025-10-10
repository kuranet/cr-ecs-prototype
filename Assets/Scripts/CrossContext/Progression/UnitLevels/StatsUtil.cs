using System.Collections.Generic;

public static class StatsUtil 
{
    public static bool IsMovingStat(StatType type)
    {
        return type == StatType.MovingSpeed;
    }

    public static bool IsDamageStat(StatType type)
    {
        return type == StatType.Damage;
    }

    public static void GetAggregatedStatsList(IEnumerable<StatsConfig> config)
    {
        var statsList = new Dictionary<StatType, List<StatsConfig>>();
        foreach (var statConfig in config)
        {
            if (!statsList.ContainsKey(statConfig.type))
            {
                statsList.Add(statConfig.type, new List<StatsConfig>());
            }
            statsList[statConfig.type].Add(statConfig);
        }
    }
}
