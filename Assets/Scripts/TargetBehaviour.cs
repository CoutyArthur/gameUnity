using UnityEngine;


public class TargetBehaviour : MonoBehaviour
{
    [SerializeField] GameObject _explosionEffect;
    [SerializeField] float _effectLifetime = 2f;
    bool _quitting;

    void OnApplicationQuit() => _quitting = true;

    void OnDestroy()
    {
        if (_quitting || !gameObject.scene.isLoaded) return;

        Debug.Log("Touché : " + gameObject.name);

        if (_explosionEffect != null)
        {
            GameObject fx = Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy(fx, _effectLifetime);
        }
    }
}
