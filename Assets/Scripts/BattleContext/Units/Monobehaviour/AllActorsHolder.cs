using UnityEngine;

public class AllActorsHolder : MonoBehaviour
{
    private static AllActorsHolder _instance;
    public static AllActorsHolder Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }
}
