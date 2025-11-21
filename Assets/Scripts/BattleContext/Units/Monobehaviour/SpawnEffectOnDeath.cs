using UnityEngine;

public class SpawnEffectOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject _spawnOnDeath;
    private Vector3 _offset = new Vector3(0, 0.5f, 0);

    private void OnDestroy()
    {
        Instantiate(_spawnOnDeath, transform.position + _offset, transform.rotation);
    }
}
