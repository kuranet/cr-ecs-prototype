using UnityEngine;

public class TowerVisuals : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] towerParts;

    public void SetMaterial(Material material)
    {
        foreach (var part in towerParts)
        {
            part.material = material;
        }
    }
}
