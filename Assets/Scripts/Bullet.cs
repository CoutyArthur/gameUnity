using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Target"))
        {
            Debug.Log("Touché : " + collider.gameObject.name);
            // ex: other.gameObject.GetComponent<Health>()?.TakeDamage(10);
            Destroy(gameObject);
        }
    }
}
