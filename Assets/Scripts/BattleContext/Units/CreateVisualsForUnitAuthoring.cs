using Unity.Entities;
using UnityEngine;

public class CreateVisualsForUnitAuthoring : MonoBehaviour
{
    public GameObject Prefab;

    class Baker : Baker<CreateVisualsForUnitAuthoring>
    {
        public override void Bake(CreateVisualsForUnitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new CreateVisualsForUnit
            {
                Prefab = authoring.Prefab,
            }); ;
        }
    }
}
public class CreateVisualsForUnit : IComponentData
{
    public GameObject Prefab;
}
