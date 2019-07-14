using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour {

    public AudioMixer masterMixer;

    public void SetSfxLvl (float sfxLvl)
    {
        masterMixer.SetFloat("sfxVol", Mathf.Log(sfxLvl)*20);
        Debug.Log("sfxLvl");
    }

    public void SetMusicLvl (float musicLvl)
    {
        masterMixer.SetFloat("musicVol",  Mathf.Log(musicLvl)*20);
        Debug.Log("musicLvl");
    }

    public void SetMasterLvl (float masterLvl)
    {
        masterMixer.SetFloat("MasterVol", Mathf.Log(masterLvl) * 20);
        Debug.Log("masterLvl");
    }
}
