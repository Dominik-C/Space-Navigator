// Manager zur Verwaltung des Raketensystems

using UnityEngine;
using System.Collections.Generic;

public abstract class LauncherManager : MonoBehaviour
{
    new Transform transform;
    public Transform spielerSchiff;
    public LauncherMissile rocketPrefab;
    public int raketenAnzahl = 1;
    public float verzögerung = 6.0f;
    public List<Transform> launchPoints;

    protected float nachladeCooldown = 0.0f;
    protected float magazinNachladeCooldown = 0.0f;

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    public AudioClip fireClip;
    public float fireVolume = 1.0f;
    public float minDistance = 10.0f;
    public float maxDistance = 200.0f;
    protected AudioSource fireSource;

    abstract public int MagazinAnzahl { get; }

    protected virtual void Awake()
    {
        transform = GetComponent<Transform>();
        if (launchPoints.Count == 0)
        {
            Transform[] potentialHPs = GetComponentsInChildren<Transform>();
            for (int i = 0; i < potentialHPs.Length; i++)
            {
                if (potentialHPs[i].name.StartsWith("Hp"))
                    launchPoints.Add(potentialHPs[i]);
            }

            if (launchPoints.Count == 0)
                launchPoints.Add(transform);
        }

        if (fireClip != null)
        {
            fireSource = gameObject.AddComponent<AudioSource>();
            fireSource.clip = fireClip;
            fireSource.minDistance = minDistance;
            fireSource.maxDistance = maxDistance;
            fireSource.loop = false;
            fireSource.dopplerLevel = 0.0f;
            fireSource.spatialBlend = 1.0f;
            fireSource.volume = fireVolume;
            fireSource.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
            fireSource.outputAudioMixerGroup = mixerGroup ?? null;
            fireSource.Stop();
        }
    }

    abstract public void Launch(Transform target);
    abstract public void Launch(Transform target, Vector3 velocity);
    abstract public void LauncherZurucksetzen();
}