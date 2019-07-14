// Entfert Rakete und Partikeleffekte nach Einschlag

using UnityEngine;

[DisallowMultipleComponent]
public class LauncherRemoveFX : MonoBehaviour
{
    ParticleSystem[] particles;
    public bool readyToDestroy = false;
    float effectStartTime = 0.0f;

    void OnEnable()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        effectStartTime = Time.time;
    }

    void Update()
    {
        // Entferne alle Partikel
        bool particleVorhanden = true;
        foreach (ParticleSystem remove in particles)
        {
            if (remove.particleCount > 0)
            {
                particleVorhanden = false;
                break;
            }
        }
 
        if (readyToDestroy && particleVorhanden && Time.time - effectStartTime > 1.0f)
            Destroy(gameObject);
    }
}
