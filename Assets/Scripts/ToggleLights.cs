// Zuständig um Spielerlicht ein und auszuschalten

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLights : MonoBehaviour {

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    protected AudioSource onSource;         // Audio Source zum einschalten des Lichts
    protected AudioSource offSource;        // Audio Source zum ausschalten des Lichts
    public AudioClip lightOn;
    public AudioClip lightOff;
    private bool lightON = false;

    // Hohle Script als GameObject, Parent Objekt muss ausgewählt werden (in Editor).
    public GameObject lightShaftScript;

    void Awake()
    {
        if (lightOn != null)
        {
            onSource = gameObject.AddComponent<AudioSource>();
            onSource.clip = lightOn;
            onSource.loop = false;
            onSource.volume = 0.6f;
            onSource.outputAudioMixerGroup = mixerGroup ?? null;
            onSource.Stop();
        }

        if (lightOff != null)
        {
            offSource = gameObject.AddComponent<AudioSource>();
            offSource.clip = lightOff;
            offSource.loop = false;
            offSource.volume = 0.6f;
            offSource.outputAudioMixerGroup = mixerGroup ?? null;
            offSource.Stop();
        }
    }

    void Start ()
    {
        this.GetComponent<Light>().enabled = false;
        lightShaftScript.GetComponent<LightShafts>().enabled = false;
    }
	

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.B) && lightON == false && GameUI.inSpiel == false)
        {           
            this.GetComponent<Light>().enabled = true;
            lightShaftScript.GetComponent<LightShafts>().enabled = true;

            if (onSource != null)   // Spiele Audio ab.
                onSource.Play();

            lightON = true;         // Licht einschalten
        }
        else if (Input.GetKeyDown(KeyCode.B) && GameUI.inSpiel == false)
        {           
            this.GetComponent<Light>().enabled = false;
            lightShaftScript.GetComponent<LightShafts>().enabled = false;

            if (offSource != null)   // Spiele Audio ab.
                offSource.Play();

            lightON = false;         // Licht ausschalten
        }
    }   
}
