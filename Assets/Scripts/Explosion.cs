// Verschiedene Effekte, Rauch und Funken bei Kollision, Explosion bei Zerstörung. Kann von Spielern wie Gegnern verwendet werden

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Explosion : MonoBehaviour {

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject smoke;
    [SerializeField] Rigidbody rigidBody;
	[SerializeField] float laserHitModifier = 100f;
	[SerializeField] Shield shield;
	[SerializeField] GameObject blowUp;
    [SerializeField] AudioClip playerExplosion;

    void IveBeenHit(Vector3 pos){
        GameObject go = Instantiate (explosion, pos, Quaternion.identity, transform) as GameObject;
		Destroy (go, 8f);

		if (shield == null) {
			return;
		}
//		Debug.Log ("getroffen");
		shield.TakeDamage ();
	}


    void SmokeAdd(Vector3 pos)
    {
        GameObject go = Instantiate(smoke, pos, Quaternion.identity, transform) as GameObject;
        Destroy(go, 7f);
    }


    // Explosion tritt auf bei Kollision mit Gegenständen
    void OnCollisionEnter(Collision collision){
		foreach (ContactPoint contact in collision.contacts) {
			IveBeenHit (contact.point);
		}
	}

	public void AddForce(Vector3 hitPosition, Transform hitSource){
		Debug.LogWarning ("AdForce: " + gameObject.name + " - "  +hitSource.name);

		IveBeenHit (hitPosition);
		if (rigidBody == null) {
			return;
		}

		Vector3 forceVector = (hitSource.position - hitPosition).normalized;
		Debug.Log (forceVector * laserHitModifier);
		rigidBody.AddForceAtPosition (forceVector.normalized * laserHitModifier, hitPosition, ForceMode.Impulse);
    }


	public void BlowUp(){
        if(CameraControls.cameraFirst == true)
        { 
            AudioSource.PlayClipAtPoint(playerExplosion, transform.position, 0.07f);
        }
        else
        {
            AudioSource.PlayClipAtPoint(playerExplosion, transform.position);
        }

        GameObject temp =  Instantiate (blowUp, transform.position, Quaternion.identity) as GameObject; //Particeleffekt getriggert
		Destroy(temp, 3f); // Zerstört am Spielende alle Prefabs Explosionen 
		Destroy (gameObject); 
	}

}
