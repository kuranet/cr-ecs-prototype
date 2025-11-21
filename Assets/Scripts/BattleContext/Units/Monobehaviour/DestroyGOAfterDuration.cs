using UnityEngine;

public class DestroyGOAfterDuration : MonoBehaviour
{
    [SerializeField] private float _duration;
    private float _liveTime;

    void Update()
    {
        _liveTime += Time.deltaTime;
        if (_liveTime >= _duration)
        {
            Destroy(gameObject);
        }
    }
}
