using Unity.Entities;

public class UsingAbilityState : IComponentData
{
    public Ability currentAbility;
    public float timeInState;
    public bool hasCast;
}