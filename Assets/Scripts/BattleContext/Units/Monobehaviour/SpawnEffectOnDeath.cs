using UnityEngine;

public class SpawnEffectOnDeath : MonoBehaviour
{
    private bool _isQuitting = false;

    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    [SerializeField] private GameObject _spawnOnDeath;
    private Vector3 _offset = new Vector3(0, 0.5f, 0);

    private void OnDestroy()
    {
        if (_isQuitting) return;
        Instantiate(_spawnOnDeath, transform.position + _offset, transform.rotation);
    }
}
