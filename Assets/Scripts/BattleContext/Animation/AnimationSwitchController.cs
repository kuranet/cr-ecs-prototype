using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class AnimationSwitchController : MonoBehaviour
{
    [SerializeField] private EntityToGOLink linker;
    [SerializeField] private Animator animController;

    private const string movingKey = "moving";
    private const string attackingKey = "attacking";

    private void Start()
    {
        if (World.DefaultGameObjectInjectionWorld.IsServer())
        {
            UnityEngine.Debug.LogWarning($"{gameObject.name} has a AnimationSwitchController on server, deactivate it");
            enabled = false;
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity e = linker.entity;

        var animState = em.GetComponentData<AnimationState>(e);
        bool moving = animState.animation == AnimationType.Moving;
        bool attacking = animState.animation == AnimationType.Melee;

        if (moving == animController.GetBool(movingKey) &&
            attacking == animController.GetBool(attackingKey))
            return;

        animController.SetBool(movingKey, moving);
        animController.SetBool(attackingKey, attacking);
    }
}
