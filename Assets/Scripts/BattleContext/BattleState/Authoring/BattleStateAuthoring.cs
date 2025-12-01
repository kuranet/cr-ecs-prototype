using Unity.Entities;
using UnityEngine;

public class BattleStateAuthoring : MonoBehaviour
{
    private class Baker : Baker<BattleStateAuthoring>
    {
        public override void Bake(BattleStateAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new BattleState());
        }
    }
}
