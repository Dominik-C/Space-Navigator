using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationSFX : MonoBehaviour {

    public GameObject hitSFX;
    Transform myT;

    void Awake()
    {
        myT = transform;
    }
    void HitByRay()
    {
        Debug.Log("Station hit by Laser Ray");        
    }
}
