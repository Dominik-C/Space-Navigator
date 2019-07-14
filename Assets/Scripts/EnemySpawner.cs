// Zuständig für das Spawnen von Gegnern nach zeitlichem Interval

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] float spawnTimer = 5f;
    bool spawnStop = false;

	void Start(){
		//StartSpawning ();
	}

	void OnEnable(){
		EventManager.onStartGame += StartSpawning; //Spawnen beginnt erst wenn speielr auf Play gedrückt hat
		EventManager.onPlayerDeath += StopSpawning; // Wenn spieler tot ist sollen keine Weiteren Enemy Spawnen
	}

	void OnDisable(){
		StopSpawning ();
		EventManager.onStartGame -= StartSpawning; 
		//EventManager.onPlayerDeath -= StopSpawning;
	}

    private void Update()
    {
        // Maximal 10 Gegener im Level (Aus Performancegründen dürfen nicht 'unendlich' viele Gegner spawnen über Zeit.
        if(PlayerInput.nearByEnemies.Count >= 10)
        {
            StopSpawning();
            spawnStop = true;
        }
        if (spawnStop && PlayerInput.nearByEnemies.Count <= 10)
        {
            StartSpawning();
            spawnStop = false;
        }
    }

    void SpawnEnemy(){
		Instantiate (enemyPrefab, transform.position, Quaternion.identity);
	}

	void StartSpawning(){
		InvokeRepeating ("SpawnEnemy", spawnTimer, spawnTimer);
	}

	void StopSpawning(){
		CancelInvoke ();
	}
}
