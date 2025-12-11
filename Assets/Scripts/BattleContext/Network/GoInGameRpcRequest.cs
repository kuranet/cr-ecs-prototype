using Unity.NetCode;

public struct GoInGameRpcRequest : IRpcCommand
{
    public int kingTowerLevel;
    public int archerTowerLevel;

    //public string kingTowerVisuals;
    //public string archerTowerVisuals;

    public int kingTowerIndex;
    public int archerTowerIndex;
}
