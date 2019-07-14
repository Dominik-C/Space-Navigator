// Zuständig für Zufallsgenerierung von Asteroiden-Transformationen, sowie prozeduraler Scenen Platzierung

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{

    [SerializeField] Asteroid asteroidPrefab;
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] int numberOfAsteroidsOnAnAxis = 10;
    [SerializeField] int gridSpacing = 100;

    float collisionRadius = 3.0f;

    public List<Asteroid> asteroid = new List<Asteroid>();

    // nach dem Klicken auf dem Play Button erschenit das Atseroiden Feld
    void OnEnable()
    {
        EventManager.onStartGame += PlaceAsteroids;
        EventManager.onPlayerDeath += DestroyAsteroids;
        EventManager.onRespawnPickup += PlacePickup;
    }

    void OnDisable()
    {
        EventManager.onStartGame -= PlaceAsteroids;
        EventManager.onPlayerDeath -= DestroyAsteroids;
        EventManager.onRespawnPickup -= PlacePickup;
    }

    void PlaceAsteroids()
    {
        for (int x = 0; x < numberOfAsteroidsOnAnAxis; x++)
        {
            for (int y = 0; y < numberOfAsteroidsOnAnAxis; y++)
            {
                for (int z = 0; z < numberOfAsteroidsOnAnAxis; z++) //Die Schleife legt das Muster fest indem die Asteroiden Spawnen
                {
                    InstantiateAsteroid(x, y, z);

                }
            }
        }
        PlacePickup();
    }


    void DestroyAsteroids()
    {
        foreach (Asteroid ast in asteroid)
        {
            ast.SelfDestruct();
        }
        asteroid.Clear();
    }

    void InstantiateAsteroid(int x, int y, int z)
    { // init der Ateroids    Vector3 sorgt dafür das alle Asteroiden in eienr Reihe sind

        Asteroid temp = Instantiate(asteroidPrefab,
            new Vector3(
                transform.position.x + (x * gridSpacing) + AsteroidOffset(),
                transform.position.y + (y * gridSpacing) + AsteroidOffset(),
                transform.position.z + (z * gridSpacing) + AsteroidOffset()),
                Quaternion.identity, transform) as Asteroid;

        temp.name = "Asteroid: " + x + "-" + y + "-" + z;
        asteroid.Add(temp); // Alle Asteroiden werden dieser Liste hinzugefügt
    }

    // Einen zufälligen Asterioden auswählen und Pickup in der Nähe spawnen.
    void PlacePickup()
    {
        int rnd = Random.Range(0, asteroid.Count);

        bool didHit = Physics.CheckSphere(asteroid[rnd].transform.position + (transform.right * 90), collisionRadius);
        if (didHit)
        {
            Debug.Log("NotGood, PlaceAgain!");
            PlacePickup();
        }
        else
        {
            Debug.Log("PickUp1stCheckGood, Place *90");
            Instantiate(pickupPrefab, asteroid[rnd].transform.position + (transform.right * 90), transform.rotation);
        }
    }

    float AsteroidOffset()
    {
        return Random.Range(-gridSpacing / 2f, gridSpacing / 2f);
    }
}


