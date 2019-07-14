// Verwaltung von Gegnerangriff auf Spieler

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAtack : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] Laser laser;

	Vector3 hitPosition;


	void Update(){
		if (!FindTarget ()) {
			return;
		}
		InFront ();
		HaveLineOfSightRayCast ();

		if (InFront () && HaveLineOfSightRayCast ()) {
			FireLaser ();
		}
	}

	bool InFront(){
		Vector3 directionToTarget = transform.position - target.position;
		float angle = Vector3.Angle (transform.forward, directionToTarget);

		//in Schussreichweite
		if(Mathf.Abs(angle) > 90 && Mathf.Abs(angle) < 270){
			return true;
		}

		return false;
	}

	bool HaveLineOfSightRayCast(){
		RaycastHit hit;

		Vector3 direction = target.position - transform.position;

		if(Physics.Raycast(laser.transform.position, direction, out hit, laser.Distance)){
			if (hit.transform.CompareTag ("Player")) {
				Debug.DrawRay (laser.transform.position, direction, Color.green);
				hitPosition = hit.transform.position;
				return true;
			}
		}

		return false;
	}

	void FireLaser(){
		laser.FireLaser (hitPosition, target);
	}

	bool FindTarget(){
		if (target == null) {
			GameObject temp = GameObject.FindGameObjectWithTag ("Player");

			if (temp != null) {
				target = temp.transform;
			}
		}
		if (target == null) {
			return false;
		}
		return true;
	}

}
