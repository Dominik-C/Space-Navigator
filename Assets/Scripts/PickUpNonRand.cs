// PickUp Klasse für alle Pickups, bis auf die Random-Platzierten. 
// Nur der Spieler darf mit Pickup interagieren.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[DisallowMultipleComponent]
//[RequireComponent(typeof(CapsuleCollider))]

public class PickUpNonRand : MonoBehaviour
{
    [SerializeField] float rotationOffset = 100f;

    bool gotHitP = false; // Damit ein Pickup nur einmalig eingesammelt werden kann
    Transform myT;
    Vector3 randomRotation;

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
        myT.Rotate(randomRotation * Time.deltaTime);
    }

    void HitByRay()
    {
        Debug.Log("Hit by Laser");
    }



}