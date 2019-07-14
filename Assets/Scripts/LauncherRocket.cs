// Rakete -> zielsuchend - Ist auf Raketenprefab anzubringen

using UnityEngine;
using System.Collections.Generic;

public class LauncherRocket : LauncherManager
{
    Queue<RaketenSystem> stations;
    int startRaketenAnzahl = 1;
    int spawnedMissiles = 0;

    public override int MagazinAnzahl { get { return 1; } }

    protected override void Awake()
    {
        base.Awake();
        stations = new Queue<RaketenSystem>(launchPoints.Count); // Warteschlange aller Raketenstationen
    }

    private void Start()
    {
        startRaketenAnzahl = raketenAnzahl;
        InitializeStations();
    }

    private void Update()
    {
        if (spawnedMissiles < startRaketenAnzahl)
        {
            foreach (RaketenSystem sta in stations)
            {
                if (sta.Update(Time.deltaTime))
                    spawnedMissiles++;
            }
        }
    }

    public override void Launch(Transform target)
    {
        Launch(target, Vector3.zero);
    }


    public override void Launch(Transform target, Vector3 velocity)
    {
        RaketenSystem launchingStation = stations.Peek();

        if (launchingStation != null)
        {
            bool launchSuccessful = launchingStation.Launch(target, velocity);

            if (launchSuccessful)
            {
                if (fireSource != null)
                    fireSource.Play();

                stations.Dequeue();
                stations.Enqueue(launchingStation);

                raketenAnzahl--;
            }
        }
    }

    // Reset Launcher
    public override void LauncherZurucksetzen()
    {
        raketenAnzahl = startRaketenAnzahl;

        foreach (RaketenSystem station in stations)
            station.ClearHardpoint();

        InitializeStations();
    }


    private void InitializeStations()
    {
        stations.Clear();
        spawnedMissiles = 0;

        foreach (Transform point in launchPoints)
        {
            RaketenSystem newStation = new RaketenSystem(verzögerung, rocketPrefab, point, spielerSchiff);
            stations.Enqueue(newStation);
            spawnedMissiles++;
        }
    }


    // Hilfsklasse zur dynamischen Erzeugung neuer Raketen.
    // Beginn internal
    public class RaketenSystem
    {
        public LauncherMissile loadedMissile;

        LauncherMissile prefab;
        Transform launchPoint;
        Transform spielerSchiff;

        float nachladen = 1.0f;
        float cooldown = 0.0f;


        public RaketenSystem(float nachladen, LauncherMissile missilePrefab, Transform launchPoint, Transform spielerSchiff)
        {
            prefab = missilePrefab;
            this.nachladen = nachladen;
            this.launchPoint = launchPoint;
            this.spielerSchiff = spielerSchiff;
            cooldown = 0.0f;

            loadedMissile = CreateMissile(launchPoint);
        }

        // Automatically reloads missiles based on a cooldown.     
        public bool Update(float deltaTime)
        {
            bool spawnedNewMissle = false;

            if (loadedMissile == null)
            {
                cooldown -= deltaTime;

                // Finished reloading, spawn new missile.
                if (cooldown <= 0.0f)
                {
                    loadedMissile = CreateMissile(launchPoint);
                    spawnedNewMissle = true;
                }
            }

            return spawnedNewMissle;
        }


        public bool Launch(Transform target, Vector3 inheritedVelocity)
        {
            bool successfulLaunch = false;
            if (loadedMissile != null)
            {
                loadedMissile.Launch(target, inheritedVelocity);
                loadedMissile = null;
                cooldown = nachladen;
                successfulLaunch = true;
            }
            return successfulLaunch;
        }

        public void ClearHardpoint()
        {
            if (loadedMissile != null)
            {
                Destroy(loadedMissile.gameObject);
                loadedMissile = null;
            }
            cooldown = 0.0f;
        }

        // Spawns a missile on a station.
        private LauncherMissile CreateMissile(Transform newParent)
        {
            LauncherMissile rakete = Instantiate(prefab, newParent);
            rakete.spielerSchiff = spielerSchiff;

            // Attach the missile to the hardpoint by its attach point if possible.
            if (rakete.attachPoint != null)
            {
                rakete.transform.localPosition = -rakete.attachPoint.localPosition;
                rakete.transform.localEulerAngles = -rakete.attachPoint.localEulerAngles;
            }
            else
            {
                rakete.transform.localPosition = Vector3.zero;
                rakete.transform.localEulerAngles = Vector3.zero;
            }

            return rakete;
        }
    } // End
}