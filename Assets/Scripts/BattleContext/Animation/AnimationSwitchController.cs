using Unity.Entities;
using UnityEngine;

public class AnimationSwitchController : MonoBehaviour
{
    [SerializeField] private EntityToGOLink linker;
    [SerializeField] private Animator animController;

    private const string movingKey = "moving";
    private const string attackingKey = "attacking";

    private void Update()
    {
        var em = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity e = linker.entity;

        bool moving = em.HasComponent<MovingState>(e);
        bool attacking = em.HasComponent<UsingAbilityState>(e);

        if (moving == animController.GetBool(movingKey) &&
            attacking == animController.GetBool(attackingKey))
            return;

        animController.SetBool(movingKey, moving);
        animController.SetBool(attackingKey, attacking);

    }
}
