// Verwaltet die Eingaben des Spielers bezüglich Waffensystem und TargetSystem. 

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    protected AudioSource laserSource;
    protected AudioSource inputSource;

    [SerializeField] Laser[] laser;         // Array aller Laser
    public float laserCooldown;
    public Transform target;                // Wird beim Abfeuern mit Zielsuche überreicht
    new Rigidbody rigidbody;                // Benötigt für Rocket velocity
    int selectedLauncher = 0;               // Ausgewählter Launcher
    public AudioClip laserSound;            // Audio, wenn Laser abgefeuert wird
    public AudioClip switchWeapon;          // Audio, bei Waffenwechsel
    private Text Ammo;                      // Benötigt für Bildschirmausgabe
    private Text SelectedLauncher;          // Benötigt für Bildschirmausgabe
    Queue<LauncherManager>[] launchers;     // Warteschlange für LauncherManager
    LauncherManager[] allLaunchers;         // Array aller vorhanden Launcher

    private float timeStamp;
    public Vector3[] weaponMountPoints;
    // Laser shot prefab
    public Transform laserShotPrefab;

    // Taget System:
    public Transform targetX;               // Transform von Gegnern zur Weitergabe bei Raketenabschuss
    public Transform targetCleaner;         // Hilfsvariable zur Abschaltung aller Gegner UI Elemente
    EnemyMovement target2;                  // Um Gegner zu holen
    bool lockedOn;                          // TagetSystem ein & ausschalten
    int lockedEnemy;                        // Gegner, welcher als Target ausgewehlt ist (Index für Liste)

    // Liste aller Gegner
    public static List<EnemyMovement> nearByEnemies = new List<EnemyMovement>();  


    void OnEnable()
    {
        EventManager.onPlayerDeath += CleanUp;
    }

    void OnDisable()
    {
        EventManager.onPlayerDeath -= CleanUp;
    }

    void Awake()
    {
        GameUI.newStart = true; // Setze True, wenn Spieler geladen wird.
        Ammo = GameObject.FindWithTag("AmmoGui").GetComponent<Text>();
        SelectedLauncher = GameObject.FindWithTag("AmmoTrackingGui").GetComponent<Text>();
        rigidbody = GetComponent<Rigidbody>();
        launchers = new Queue<LauncherManager>[2];

        for (int i = 0; i < 2; i++)
            launchers[i] = new Queue<LauncherManager>();

        if (laserSound != null)
        {
            laserSource = gameObject.AddComponent<AudioSource>();
            laserSource.clip = laserSound;
            laserSource.loop = false;
            laserSource.volume = 0.7f;
            laserSource.pitch = Random.Range(0.9f, 1.3f);
            laserSource.outputAudioMixerGroup = mixerGroup ?? null;
            laserSource.Stop();
        }

        if (switchWeapon != null)
        {
            inputSource = gameObject.AddComponent<AudioSource>();
            inputSource.clip = switchWeapon;
            inputSource.loop = false;
            inputSource.volume = 0.1f;
            inputSource.outputAudioMixerGroup = mixerGroup ?? null;
            inputSource.Stop();
        }
    }

    void Start()
    {
        lockedOn = false;
        lockedEnemy = 0;

        allLaunchers = GetComponentsInChildren<LauncherManager>();

        // Register launchers
        foreach (LauncherManager launcher in allLaunchers)
        {
            if (launcher.name.StartsWith("Right"))
                launchers[0].Enqueue(launcher);

            else if (launcher.name.StartsWith("Left"))
                launchers[1].Enqueue(launcher);
        }
    }

    // Entleert die Gegnerliste
    void CleanUp()
    {
        nearByEnemies.Clear();
    }


    // Wenn Spieler das "Ammo Pickup" trifft.
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "RocketPickup")
        {
            foreach (LauncherManager launcher in allLaunchers)
                launcher.LauncherZurucksetzen();

            UpdateAmmoCounters();
        }
    }

    void Update()
    {

        if (nearByEnemies.Count > 0)
        {
            // Sobald ein Gegner in der Scene ist, wird das Target System aktiv.
            if (!lockedOn)
            {               
                if (nearByEnemies.Count >= 1)
                {
                    lockedOn = true;
                    lockedEnemy = 0;  //Der erste Gegner wird als Target gesetzt.
                }
            }
            // Wenn keine Gegner auf der Liste ist, wird das Target System deaktiviert.
            else if (lockedOn && nearByEnemies.Count <= 0)
            {
                lockedOn = false;
                lockedEnemy = 0;
                targetX = null;
            }

            // Tab drücken, um Auswahl zu wechseln.
            if (Input.GetKeyDown(KeyCode.Tab) && nearByEnemies.Count > 0)
            {
                if (nearByEnemies[lockedEnemy] == null)
                    lockedOn = false;

                if (lockedEnemy == nearByEnemies.Count - 1)  // Wenn das Ende der Liste erreicht wurde, wieder auf ersten Gegner schalten.
                    lockedEnemy = 0;
                else                                         // Schalte den nächsten Gegner als Target, wenn noch weitere Einträge in der Liste vorhanden sind.
                    lockedEnemy++;
            }
        }
        else
            lockedOn = false;

        if (lockedOn)
        {
            // Stellt sicher, das Listenindex keinen ungültigen Wert annehmen kann.
            if (lockedEnemy > nearByEnemies.Count -1)
            {
               // EnemyGuiCleaner();
                if (nearByEnemies.Count >= 1)
                {
                    lockedEnemy = nearByEnemies.Count -1;
                    targetX = nearByEnemies[nearByEnemies.Count-1].transform;
                }
                else
                {           
                    lockedEnemy = 0;
                    targetX = nearByEnemies[lockedEnemy].transform;
                }
            }

            if (nearByEnemies.Count != 0)
            {
                if (nearByEnemies[lockedEnemy] == null)
                {
                    // Setze Gegner zurück auf den Ersten und deaktiviere das System
                    EnemyGuiCleaner();
                    lockedEnemy = 0;
                    lockedOn = false;
                }

                EnemyGuiCleaner();                                                  // Entfernt vorheriges Target GUI
                targetX = nearByEnemies[lockedEnemy].transform;                     // Target Transform für eigentliches Tracking
                targetX.Find("TargetGui").GetComponent<Renderer>().enabled = true;  // Aktiviert Target GUI für den ausgewählten Gegner
            }
        }

        // Laser abfeuern mit Leertaste oder Mausrad Klick
        if (Input.GetKey(KeyCode.Space) && GameUI.inSpiel == false || Input.GetKey(KeyCode.Mouse2) && GameUI.inSpiel == false)
        {

            if (timeStamp <= Time.time)
            {
                timeStamp = Time.time + laserCooldown; // Adds coolsdown
                if (laserSource != null)   // Spiele Audio ab.
                    laserSource.Play();

                foreach (Laser l in laser)
                {
                    Vector3 pos = transform.position + (transform.forward * l.Distance);
                    l.FireLaser();
                }

                // Calculate where the position is in world space for the mount point & instantiate at position with the spaceships rotation
                foreach (Vector3 _wmp in weaponMountPoints)
                {
                    Vector3 _pos = transform.position + transform.right * _wmp.x + transform.up * _wmp.y + transform.forward * _wmp.z;
                    Transform _laserShot = (Transform)Instantiate(laserShotPrefab, _pos, transform.rotation);
                    _laserShot.GetComponent<SU_LaserShot>().firedBy = transform;
                }
            }
        }

        // Zwischen Launchern wechseln, mit linker Maus oder Alt.
        if (Input.GetButtonDown("Fire2") && GameUI.inSpiel == false)
        {
            if (inputSource != null)    // Spiele Audio ab.
                inputSource.Play();

            selectedLauncher++;        // Launcher wechseln

            if (selectedLauncher >= 2) // Damit nur zwischen den vorhandenen Launchern gewechselt werden kann
                selectedLauncher = 0;
        }

        // Ausgewählten Launcher abfeuern
        if (selectedLauncher == 1 && nearByEnemies.Count > 0) // Wenn Potenzielles Target in Scene nutze FireWeapon
        {
            if (Input.GetButton("Fire1") && GameUI.inSpiel == false)
                FireWeapon();
        }
        else if (selectedLauncher == 0 && nearByEnemies.Count > 0)
        {
            if (Input.GetButtonDown("Fire1") && GameUI.inSpiel == false)
                FireWeapon();
        }

        if (selectedLauncher == 1 && nearByEnemies.Count == 0)    // Wenn kein Target in Scene nutze FireWeaponTemp
        {
            if (Input.GetButton("Fire1") && GameUI.inSpiel == false)
                FireWeaponTemp();
        }
        else if (selectedLauncher == 0 && nearByEnemies.Count == 0)
        {
            if (Input.GetButtonDown("Fire1") && GameUI.inSpiel == false)
                FireWeaponTemp();
        }
        UpdateAmmoCounters();
    }


    // Wird benötigt um alle vorherig aktivierten "TargetGUI Elemente" abzuschalten.
    private void EnemyGuiCleaner()
    {
        for (int i = 0; i < nearByEnemies.Count; ++i)
        {
            targetCleaner = nearByEnemies[i].transform;
            targetCleaner.Find("TargetGui").GetComponent<Renderer>().enabled = false;
        }
    }

    // Launcher abfeuern, wenn min. 1 Gegner in der Scene ist.
    private void FireWeapon()
    {
        if (launchers[selectedLauncher].Count > 0)
        {
            target = targetX;                                               // Bei Abschuss wird das Ziel an Laucher übergeben
            LauncherManager temp = launchers[selectedLauncher].Dequeue();   // Von Warteschlange abmelden
            temp.Launch(target, rigidbody.velocity);                        // Abschuss findet statt
            launchers[selectedLauncher].Enqueue(temp);                      // Von Warteschlange anmelden
        }
    }

    // Launcher abfeuern, auch wenn keine Gegner in der Scene sind. Wen kein Gegner in Scene ist, wurde targetX bereits auf NULL gesetzt.
    private void FireWeaponTemp()
    {
        if (launchers[selectedLauncher].Count > 0)
        {
            LauncherManager temp = launchers[selectedLauncher].Dequeue();
            temp.Launch(targetX, rigidbody.velocity);
            launchers[selectedLauncher].Enqueue(temp);
        }
    }

    // Munitionsanzeige updaten 
    private void UpdateAmmoCounters()
    {
        int raketenAnzahl = 0;              // Zielsuchend
        int rocketCount = 0;                // Nicht Zielsuchend
        int rocketMagazine = 0;             // Magazine für nicht Zielsuchende

        foreach (LauncherManager launcher in allLaunchers)
        {
            if (launcher.name.StartsWith("Right"))
                raketenAnzahl += launcher.raketenAnzahl;

            else if (launcher.name.StartsWith("Left"))
            {
                rocketCount += launcher.raketenAnzahl;
                rocketMagazine += launcher.MagazinAnzahl;
            }
        }

        // Bildschirmausgabe 
        if (selectedLauncher == 0)
        {
            SelectedLauncher.text = "Tracking Missile:  " + raketenAnzahl.ToString("0") + "\n";
            Ammo.text = "Standard Missile: " + rocketCount.ToString("0") + "/" + rocketMagazine.ToString("0");
        }
        else
        {
            Ammo.text = "Tracking Missile:  " + raketenAnzahl.ToString("0") + "\n";
            SelectedLauncher.text = "Standard Missile: " + rocketCount.ToString("0") + "/" + rocketMagazine.ToString("0");
        }
    }
}
