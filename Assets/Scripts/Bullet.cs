using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject _explosionEffect;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Target"))
        {
            Debug.Log("Touché : " + collider.gameObject.name);
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
            Destroy(collider.gameObject);
            Destroy(gameObject);
        }
    }
}