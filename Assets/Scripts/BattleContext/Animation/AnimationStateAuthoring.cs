using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class AnimationStateAuthoring : MonoBehaviour
{
    class Baker : Baker<AnimationStateAuthoring>
    {
        public override void Bake(AnimationStateAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new AnimationState
            {
                animation = AnimationType.Idle
            }) ;
        }
    }
}

public struct AnimationState : IComponentData
{
    [GhostField] public AnimationType animation;
}

public enum AnimationType
{
    None, Idle, Moving, Melee
}