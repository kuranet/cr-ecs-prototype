using UnityEngine;
using Unity.Entities;

public class NavAgentDataAuthoring : MonoBehaviour
{
    public class NavAgentAuthoringBaker : Baker<NavAgentDataAuthoring>
    {
        public override void Bake(NavAgentDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            UnityEngine.Debug.LogError($"add NavAgentDataAuthoring tag on {entity}");
            AddComponent(entity, new NavAgentData
            {
                Destination = authoring.transform.position,
            }) ;
        }
    }
}
