using Graphical.AnimationWithGameObjects;
using Unity.Entities;
using Unity.Transforms;

public partial struct UsingAbilityStateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, usingAbilityState, entity) in
                 SystemAPI.Query<RefRO<LocalTransform>, UsingAbilityState>()
                 .WithEntityAccess()
                 .WithAll<UnitTag>())
        {
            usingAbilityState.timeInState += SystemAPI.Time.DeltaTime;

            if (!usingAbilityState.hasCast && usingAbilityState.timeInState > usingAbilityState.currentAbility.castDelay)
            {
                var createdEntity = state.EntityManager.Instantiate(usingAbilityState.currentAbility.prefab);

                var localTrans = SystemAPI.GetComponentRW<LocalTransform>(createdEntity);
                localTrans.ValueRW.Position = transform.ValueRO.Position;

                usingAbilityState.hasCast = true;
                UnityEngine.Debug.LogError($"after {usingAbilityState.timeInState} actually cast ability so");
            }
        }
    }
}
