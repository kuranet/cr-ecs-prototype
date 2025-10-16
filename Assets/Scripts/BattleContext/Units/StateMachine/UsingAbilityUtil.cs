using Unity.Entities;
using System.Collections.Generic;
using Unity.Transforms;
using Unity.Mathematics;

public static class UsingAbilityUtil 
{
    public static List<Entity> GetUnitAbilitiesBuffer(Entity unit, EntityManager em)
    {
        var readyBuffer = new List<Entity>();
        if (em.HasBuffer<AbilityChargeBuffer>(unit) == false)
        {
            return readyBuffer;
        }

        var allAbilitiesList = em.GetBuffer<AbilityChargeBuffer>(unit);

        for (int i = 0; i < allAbilitiesList.Length; i++)
        {
            var abilityState = em.GetComponentData<Ability>(allAbilitiesList[i].AbilityEntity);
            if (allAbilitiesList[i].timeInCooldown < abilityState.cooldown)
            {
                continue;
            }

            readyBuffer.Add(allAbilitiesList[i].AbilityEntity);
        }

        return readyBuffer;
    }

    public static bool IsInMovementAbilityRange(Entity unit, EntityManager em)
    {
        if (em.HasBuffer<AbilityChargeBuffer>(unit) == false)
        {
            return false;
        }

        if (!em.HasComponent<Target>(unit))
        {
            return false;
        }

        var target = em.GetComponentData<Target>(unit);
        var targetLocalTransform = em.GetComponentData<LocalTransform>(target.Object);

        var allAbilitiesList = em.GetBuffer<AbilityChargeBuffer>(unit);
        var localTransform = em.GetComponentData<LocalTransform>(unit);

        for (int i = 0; i < allAbilitiesList.Length; i++)
        {
            var abilityState = em.GetComponentData<Ability>(allAbilitiesList[i].AbilityEntity);
            if (math.distance(localTransform.Position, targetLocalTransform.Position) < abilityState.range)
            {
                return true;
            }
        }

        return false;
    }
}
