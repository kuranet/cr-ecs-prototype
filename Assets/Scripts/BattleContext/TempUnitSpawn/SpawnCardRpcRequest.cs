using Unity.Mathematics;
using Unity.NetCode;

public struct SpawnCardRpcRequest : IRpcCommand
{
    public int localPlayerIndex;
    public float3 worldPoint;
    public int playedCard;
}