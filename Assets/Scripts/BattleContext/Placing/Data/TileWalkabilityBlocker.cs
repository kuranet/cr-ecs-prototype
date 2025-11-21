using Unity.Entities;

public struct TileWalkabilityBlocker : IComponentData
{
    public int blockedLength;
    public int blockedWidth;
}
