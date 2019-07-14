// Hauptklasse der Gegner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(EnemyAtack))]
[RequireComponent(typeof(TrailRenderer))]

public class EnemyMovement : MonoBehaviour {

	[SerializeField] Transform target;                  // Target Transform (Spielerschiff)
	[SerializeField] float rotationlDamp = 0.5f;        // Rotationszeit
	[SerializeField] float movementSpeed = 10f;         // Geschwindigkeit
	[SerializeField] float rayCastOffset = 2.5f;        // Wartezeit nach Schießen
	[SerializeField] float detectionDistance = 20f;     // Reichweite des Gegnerwaffen
    private int life = 600;                             // Lebensenergie eines Gegners
    [SerializeField] GameObject ExplosionHitEffect;
    [SerializeField] GameObject LaserHitEffect;
    [SerializeField] GameObject blowUpEffect;
    [SerializeField] GameObject AmmoDrop;
    [SerializeField] GameObject FuelDrop;

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    public AudioClip hitByLaserSound;
    public AudioClip explosionSound;
   
    bool addOnlyOnce;   // Damit Gegner nur einmal in die Liste kommt
    bool inScene;       // Um sicher zu sein, dass Gegner auch in Scene ist

    void OnEnable(){
		EventManager.onPlayerDeath += FindMainCamera;
		EventManager.onPlayerDeath += SelfDestruct;
	}

	void OnDisable(){
		EventManager.onPlayerDeath -= FindMainCamera;
		EventManager.onPlayerDeath -= SelfDestruct;
	}

    void Start()
    {
        inScene = true;
        addOnlyOnce = true;
    }

    // Zerstört alle Enemies bei Spielertot, damit es in der nächsten runde bei 0 Enemy anfängt --> Listeneintrag wird 
    void SelfDestruct()
    {
        Destroy (gameObject); 
	}

    void OnCollisionEnter(Collision health)     // Wenn ein Gegner von einer Rakete getroffen wird, geht seine Lebensenergie auf 0
    {
        if (health.gameObject.tag == "launcher")
        {
            life -= 600;
            GameObject explosionSFX = Instantiate(ExplosionHitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(explosionSFX, 2.1f);
        }
    }

    void HitByRay()     // Wenn ein Gegner von einer Laser getroffen wird, wird 150 Lebensenergie abgezogen. 
    {
        life -= 150;
        GameObject laserSFX = Instantiate(LaserHitEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(laserSFX, 1f);
        Debug.Log("Enemy Life = " + life);
        AudioSource.PlayClipAtPoint(hitByLaserSound, transform.position);
        if (life <= 0)
        {     // Wenn die Lebensenergie 0 ist führe "Kill" funktion aus.
            GameObject explosion2SFX = Instantiate(ExplosionHitEffect, transform.position, Quaternion.identity) as GameObject;
            Destroy(explosion2SFX, 1f);
        }
    }

    //Gegner töten
    void Kill()
    {
        GameObject explosionSFX = Instantiate(blowUpEffect, transform.position, Quaternion.identity) as GameObject; // Particeleffekt getriggert
        Destroy(explosionSFX, 1.9f);                                        // Entferne Explosion, nach 2 Sekunden
        PlayerInput.nearByEnemies.Remove(this);                             // Entferne Gegner von Liste   
        randomDrop();                                                       // Randomizer aufrufen
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);    // Spiele Audio ab
        EventManager.ScorePoints(200);                                      // Spieler erhält 200 Punkte
        Destroy(gameObject);                                                // Objekt entfernen
    }

    // Randomizer, denn nicht jeder Gegner soll Ammo fallen lassen
    void randomDrop()
    {
        int num = Random.Range(0, 60);
        if (num > 35) { 
            dropAmmo();
        }
        if (num < 10) { 
            dropFuel();
        }
    }

    // Ammo drop
    void dropAmmo()
    {
        GameObject itemDrop = Instantiate(AmmoDrop, transform.position, Quaternion.identity) as GameObject; 
    }

    void dropFuel()
    {
        GameObject itemDrop = Instantiate(FuelDrop, transform.position, Quaternion.identity) as GameObject;
    }

    void Update()
    {
        //Wenn ein Gegner in die Scene kommt, füge diesen einmalig zur Liste hinzu. 
        if (inScene && addOnlyOnce)
        {
            addOnlyOnce = false;
            PlayerInput.nearByEnemies.Add(this);
        }

        if (life <= 0){     // Wenn die Lebensenergie 0 ist führe "Kill" funktion aus.
            Kill();
        }

        if (!FindTarget ()) {   //Tue nichts, wenn kein Spielerschiff in Scene ist 
			return;
		}

		Pathfinding ();  
        Turn ();
		Move ();
	}

	// dreht sich zum Spieler
	void Turn(){
		Vector3 pos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation (pos);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationlDamp * Time.deltaTime);
	}

	void Move(){
		transform.position += transform.forward * movementSpeed * Time.deltaTime;
	}

	// Asteroiden suchen und erkennen ggf ausweichen
	void Pathfinding(){
		RaycastHit hit;
		Vector3 raycastOffset = Vector3.zero;

		Vector3 left = transform.position - transform.right * rayCastOffset;
		Vector3 right = transform.position + transform.right * rayCastOffset;
		Vector3 up = transform.position + transform.up * rayCastOffset;
		Vector3 down = transform.position - transform.up * rayCastOffset;

		if (Physics.Raycast (left, transform.forward, out hit, detectionDistance)) {
			raycastOffset += Vector3.right;
		}
		else if (Physics.Raycast (right, transform.forward, out hit, detectionDistance)) {
			raycastOffset -= Vector3.right;
		}
		if (Physics.Raycast (up, transform.forward, out hit, detectionDistance)) {
			raycastOffset -= Vector3.up;
		}
		else if (Physics.Raycast (down, transform.forward, out hit, detectionDistance)) {
			raycastOffset += Vector3.up;
		}

		if (raycastOffset != Vector3.zero) {
			transform.Rotate (raycastOffset * 5f * Time.deltaTime);
		} else {
			Turn ();
		}
	}

    // Findet den Spieler.
    bool FindTarget()
    {
		if (target == null){
			GameObject temp = GameObject.FindGameObjectWithTag("Player");
			if (temp != null) {
				target = temp.transform;
			}
		}
		if (target == null){
			return false;
		}
		return true;
	}

	//Finde die aktive Kamera nach Spielertot.
	void FindMainCamera(){
		target = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
}
