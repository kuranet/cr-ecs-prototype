using Unity.Entities;
using UnityEngine;

public class AttackTargetsAuthority: MonoBehaviour
{
    public TargetType AllowedTypes;

    private class Baker : Baker<AttackTargetsAuthority>
    {
        public override void Bake(AttackTargetsAuthority authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new AttackTargets { AllowedTypes = authoring.AllowedTypes });
        }
    }
}

public struct AttackTargets : IComponentData
{
    public TargetType AllowedTypes;
}

