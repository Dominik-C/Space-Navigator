// PickUp Klasse, hauptsächlich für die Zerstörung eines Pickiups zuständig, bei Spielerkontakt. 
// Nur der Spieler darf mit Pickup interagieren.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class PickUp : MonoBehaviour
{
    [SerializeField] float rotationOffset = 100f;

    bool gotHit = false; // Damit ein Pickup nur einmalig eingesammelt werden kann
    Transform myT;
    Vector3 randomRotation;

    bool addOnlyOnePickup;   // Damit Gegner nur einmal in die Liste kommt
    bool inScenePickup;       // Um sicher zu sein, dass PickUp auch in Scene ist


    void OnEnable()
    {
        EventManager.onPlayerDeath += SelfDestruct;
    }

    void OnDisable()
    {
        EventManager.onPlayerDeath -= SelfDestruct;
    }

    // Zerstört alle Pickups bei Spielertot, damit keine alten Pickups in der Scene bleiben.
    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    void Awake()
    {
        addOnlyOnePickup = true;
        inScenePickup = true;
        myT = transform;
    }

    void Start()
    {
        // Starte zufällige Rotation
        randomRotation.x = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.y = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.z = Random.Range(-rotationOffset, rotationOffset);
    }

    void Update()
    {
        if (inScenePickup && addOnlyOnePickup)
        {
            if (Shield.pickUpList.Count > 2)
            {
                for(int i=0; i< Shield.pickUpList.Count-1; i++)
                SelfDestruct();
            }
            else if(Shield.pickUpList.Count == 2)
            {
                Shield.pickUpList.Clear();
            }
            else
            {
                inScenePickup = false;
                Shield.pickUpList.Add(this);
            }
        }

        myT.Rotate(randomRotation * Time.deltaTime);
    }

    void HitByRay(){
        Debug.Log("Hit by Laser");
    }

}
