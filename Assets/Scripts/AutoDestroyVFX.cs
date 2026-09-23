using UnityEngine;

public class AutoDestroyVFX : MonoBehaviour
{
    void Start()
    {
        ParticleSystem[] allSystems = GetComponentsInChildren<ParticleSystem>();
        float maxDuration = 0f;

        foreach (var ps in allSystems)
        {
            float duration = ps.main.duration + ps.main.startLifetime.constantMax;
            if (duration > maxDuration)
                maxDuration = duration;
        }

        Destroy(gameObject, maxDuration);
    }
}