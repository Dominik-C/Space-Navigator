// Zusätzlicher Effekt beim fliegen

using UnityEngine;

public class WindTrail : MonoBehaviour
{
    public float speed;

    TrailRenderer trail;
    Material trailMat;

    void Start()
    {
        trail = GetComponentInChildren<TrailRenderer>();
        trailMat = trail.material;
    }

    // trail updaten
    void Update()
    {
        Vector2 offset = trailMat.mainTextureOffset;

        offset.x -= Time.deltaTime * speed / 60.0f;
        if (offset.x < -1f)
            offset.x = 1f;

        trailMat.mainTextureOffset = offset;
    }
}