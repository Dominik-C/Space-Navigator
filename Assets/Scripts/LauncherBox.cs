// Launcher Box. Liegt auf LauncherBox Objekt. (Pod)

using UnityEngine;

public class LauncherBox : LauncherManager
{
    public float streuung = 0.0f;        // Höhere Nummer = Mehr Streuung
    public int magazineCount = 1;               // Anzahl zusätzlicher Magazine - optional
    public float nachladeZeit = 6.0f;     // Zeit bis Magazin nachgeladen wurde
    int boxAnzahl = 0; // Anzahl der Raketen in Magazin-Box
    int startRaketen = 1;
    int startMagazine = 1;

    private void Start()
    {
        startRaketen = raketenAnzahl;
        startMagazine = magazineCount;
    }

    private void Update()
    {
        if (nachladeCooldown > 0.0f)                               // Während Cooldown
        {
            nachladeCooldown -= Time.deltaTime;
            if (nachladeCooldown <= 0.0f)
                nachladeCooldown = 0.0f;
        }

        if (magazineCount > 0 && magazinNachladeCooldown > 0.0f)  // Während Nachladen
        {
            magazinNachladeCooldown -= Time.deltaTime;

            if (magazinNachladeCooldown <= 0.0f)                 // Wenn Nachladen fertig ist.
            {
                raketenAnzahl = startRaketen;
                nachladeCooldown = 0.0f;
                magazineCount--;
            }
        }
    }


    public override void Launch(Transform target)    // Rakete abfeuern.
    {
        Launch(target, Vector3.zero);
    }

    public override void Launch(Transform target, Vector3 velocity)    // Rakete abfeuern.
    {
        if (raketenAnzahl > 0 && nachladeCooldown <= 0.0f && magazinNachladeCooldown <= 0.0f)
        {
            if (fireSource != null)
                fireSource.Play();

            Vector3 abweichung = UnityEngine.Random.insideUnitCircle * (streuung * Mathf.Deg2Rad);
            abweichung = launchPoints[boxAnzahl].TransformDirection(abweichung);
            Vector3 randomizedForward = launchPoints[boxAnzahl].forward + abweichung;
            Quaternion randomizedRotation = Quaternion.LookRotation(randomizedForward);

            LauncherMissile missile = ErzeugeRackete(launchPoints[boxAnzahl].position, randomizedRotation);
            missile.target = target;
            missile.Launch(target, velocity);
            nachladeCooldown = verzögerung;
            raketenAnzahl--;

            boxAnzahl++;
            if (boxAnzahl >= launchPoints.Count)
                boxAnzahl = 0;
            // Reload
            if (raketenAnzahl <= 0)
                ReloadMagazine();
        }
    }

    public override void LauncherZurucksetzen()     // Setzt Launcher zurück.
    {
        nachladeCooldown = 0.0f;
        magazinNachladeCooldown = 0.0f;
        raketenAnzahl = startRaketen;
        magazineCount = startMagazine;
    }

    public void ReloadMagazine()            // Funktion für Nachladen
    {
        magazinNachladeCooldown = nachladeZeit;
    }

    public override int MagazinAnzahl { get { return magazineCount; } }

    private LauncherMissile ErzeugeRackete(Vector3 position, Quaternion rotation)      // Erzeugt Missile und feuert diese ab. 
    {
        LauncherMissile rakete = Instantiate(rocketPrefab) as LauncherMissile;
        rakete.spielerSchiff = spielerSchiff;
        rakete.transform.position = position;
        rakete.transform.rotation = rotation;
        return rakete;
    }
}