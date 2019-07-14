using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThusterSoundFX : MonoBehaviour {

    AudioSource audioThruster;
    // Use this for initialization
    void Start()
    {
        audioThruster = GetComponent<AudioSource>();
        audioThruster.Pause();
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKey(KeyCode.W) && GameUI.inSpiel == false && GameUI.inMenu == false)
        {
            audioThruster.UnPause();
        }
        else
        {
            audioThruster.Pause();
        }
    }
}
