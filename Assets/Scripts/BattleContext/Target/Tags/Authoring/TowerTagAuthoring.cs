using Unity.Entities;
using UnityEngine;

public class TowerTagAuthoring : MonoBehaviour
{
    private class Baker : Baker<TowerTagAuthoring>
    {
        public override void Bake(TowerTagAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TowerTag());

            var bridge = authoring.GetComponent<EntityToGOLink>();
            if (bridge == null)
                bridge = authoring.gameObject.AddComponent<EntityToGOLink>();

            bridge.entity = entity;
        }
    }

}
