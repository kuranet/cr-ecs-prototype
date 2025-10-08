
using Unity.Entities;

[System.Flags]
public enum TargetType : byte
{
    None = 0,
    Units = 1 << 0,
    Towers = 1 << 1,
    Flying = 1 << 2,
    All = 0xFF
}
public static class TargetUtils
{
    public static TargetType GetTargetType(Entity e, EntityManager em)
    {
        if (em.HasComponent<UnitTag>(e)) return TargetType.Units;
        if (em.HasComponent<TowerTag>(e)) return TargetType.Towers;
        if (em.HasComponent<FlyingTargetTag>(e)) return TargetType.Flying;
        return TargetType.None;
    }
}