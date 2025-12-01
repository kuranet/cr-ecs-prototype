using Unity.Entities;

public struct BattleState : IComponentData
{
    public BattleStateType State;
    public float timeInState;

    public bool isStateValidForPlacingUnits() => State == BattleStateType.Active;
}
