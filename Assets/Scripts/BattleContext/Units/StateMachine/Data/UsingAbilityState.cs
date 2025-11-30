using Unity.Entities;
using Unity.Mathematics;

public class UsingAbilityState : IComponentData
{
    public Ability currentAbility;
    public float timeInState;
    public bool hasCast;
    public Entity target;
    public float3 targetPosition;
}