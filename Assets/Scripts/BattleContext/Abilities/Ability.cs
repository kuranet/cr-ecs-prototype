using Unity.Entities;

public class Ability : IComponentData
{
    public float range;
    public float cooldown;
    public float castDelay;
    public float castDuration;
    public Entity prefab;
}