using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour
{
    [SerializeField] private List<Overlay> preparedOverlayPrefabs = new List<Overlay>();

    private readonly List<Overlay> instantiatedOverlayList = new List<Overlay>();

    private void Awake()
    {
        foreach (var overlay in preparedOverlayPrefabs)
        {
            var instance = Instantiate(overlay, transform);
            instantiatedOverlayList.Add(instance);
        }
    }
}
