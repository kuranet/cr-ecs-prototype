using Unity.Entities;

public class UsingAbilityState : IComponentData, IUnitState
{
    public Ability currentAbility;
    public float timeInState;
    public bool hasCast;
}