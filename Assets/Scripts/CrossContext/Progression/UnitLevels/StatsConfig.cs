using System;
using Unity.Entities;

[Serializable]
public struct StatsConfig : IBufferElementData
{
    public StatType type;
    public float addedValue;
    public float multipliedValue;
}
