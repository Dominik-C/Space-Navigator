// Klasse für Asteroiden

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Explosion))]

public class Asteroid : MonoBehaviour {

	[SerializeField] float minScale = .8f;              // Minimale Größe
	[SerializeField] float maxScale = 1.2f;             // Maximale Größe
    [SerializeField] float rotationOffset = 100f;       // Rotation Offset
    public GameObject destroyedVersion;                 // Asteroid Debris Model

    public static float destructionDelay = 1.0f;        // Zeit die vergeht, bis Object bei Spielende zerstört wird. (In Sekunden)

	Transform myT;
	Vector3 randomRotation;

	void Awake(){
		myT = transform;
	}

	void Start(){
		Vector3 scale = Vector3.one;
		scale.x = Random.Range (minScale, maxScale);  
		scale.y = Random.Range (minScale, maxScale);
		scale.z = Random.Range (minScale, maxScale);
		myT.localScale = scale; // Erzeugt Asteriod und skalliert Object auf den X-Y-Z-Achsen innerhalb des gültigen Bereichs

        randomRotation.x = Random.Range (-rotationOffset, rotationOffset);      // Zufälltige Rotation festlegen
		randomRotation.y = Random.Range (-rotationOffset, rotationOffset);
		randomRotation.z = Random.Range (-rotationOffset, rotationOffset);
	}

	void Update(){
		myT.Rotate (randomRotation * Time.deltaTime);  // Update jeden Frame
	}

    void HitByRay()
    {
        Debug.Log("Astroid Hit by Laser");
        //Instantiate(destroyedVersion, myT.transform.position, transform.rotation);
    }

    // Aseroiden gehen zur unterschiedlichen Zeiten kaputt nachdem der Spieler verloren hat
    public void SelfDestruct()
    {
		float timer = Random.Range (0, destructionDelay);
		Invoke ("GoBoom", timer);
	}

	public void GoBoom(){
		GetComponent<Explosion> ().BlowUp ();
	}
}
