using Unity.Entities;
using Unity.NetCode;

public struct TowerActiveState : IComponentData
{
    [GhostField] public bool isActive;
}
