using Unity.Entities;
using UnityEngine;

public class UnitTagAuthoring : MonoBehaviour
{
    private class Baker : Baker<UnitTagAuthoring>
    {
        public override void Bake(UnitTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            UnityEngine.Debug.LogError($"add unity tag on {entity}");
            AddComponent(entity, new UnitTag());
            AddComponent(entity, new IdleState());
            AddComponent(entity, new CanAttack());
        }
    }

}