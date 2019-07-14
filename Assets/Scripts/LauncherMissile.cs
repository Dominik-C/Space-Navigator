// Rakete -> Zielsuchend. Ist auf Raketenprefab anzubringen
// Zielsuche abschalten, mit suchKegel und suchReichweite = 0 im Editor.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(LauncherSFX))]
public class LauncherMissile : MonoBehaviour
{
    new Transform transform;
    new Rigidbody rigidbody;
    new CapsuleCollider collider;

    public Transform target;
    public Transform spielerSchiff;
    public Transform attachPoint;

    public float suchKegel = 45.0f; // Nur Ziele, welche sich innerhalb des Kegels befinden werden verfolgt 
    public float suchReichweite = 5000.0f; // Reichweite des SuchKegels
    public float startGeschwindigkeit = 0.0f;
    public float antriebsZeit = 3.0f;
    public float beschleunigung = 15.0f;
    public float drehung = 45.0f;
    public float lebenszeit = 15.0f;

    LauncherSFX missileSFX;
    //private Vector3 launchVelocity = Vector3.zero;
    private float abschussZeit = 0.0f;
    private float aktivierungszeit = 0.0f;
    private float geschwindigkeit = 0.0f;
    private bool abgefeuert = false;
    private bool imFlug = false;
    private bool antrieb = false;
    private bool zielErfasst = true;
    private Quaternion guidedRotation;

    public bool MissileLaunched { get { return abgefeuert; } }
    public bool AntriebAktiv { get { return antrieb; } }

    private void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        missileSFX = GetComponent<LauncherSFX>();
    }

    private void Start()
    {
        // Verhindert, dass Rakete mit Spieler Schiff Kollidiert.
        if (spielerSchiff != null)
        {
            foreach (Collider col in spielerSchiff.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(collider, col);
        }

        // Wenn Rakete noch nicht abgefeuert wurde, stelle sicher, dass sie "kinematic" ist.
        if (!abgefeuert)
            rigidbody.isKinematic = true;
    }


    private void FixedUpdate()
    {
        // Starte Zielverfolgung, wenn ein Ziel gegeben ist.
        if (imFlug && target != null)
            MissileGuidance();

        Abschusssequenz();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verhindert frühzeitige Explosion
        if (abgefeuert && TimeSince(abschussZeit) > 0)    
            DestroyMissile(true);
    }

    // Rakete Abfeuern
    public void Launch(Transform newTarget)
    {
        Launch(newTarget, Vector3.zero);
    }

    // Rakete Abfeuern
    public void Launch(Transform newTarget, Vector3 inheritedVelocity)
    {
        if (!abgefeuert)
        {
            abgefeuert = true;
            abschussZeit = Time.time;
            transform.parent = null;
            target = newTarget;
            rigidbody.isKinematic = false;
            ActivateMissile();
        }
    }

    // Abschusssequenz
    private void Abschusssequenz()
    {
        if (abgefeuert)
        {
            // Aktiviert die Rakete erst, wenn abgefeuert wurde.
            if (!imFlug && TimeSince(abschussZeit) > 0.0f)
                ActivateMissile();

            if (imFlug)
            {
                // Antrieb ist nur während Lebenszeit aktiv.
                if (antriebsZeit > 0.0f && TimeSince(aktivierungszeit) > antriebsZeit)
                    antrieb = false;
                else
                    antrieb = true;

                if (antrieb == true)
                    geschwindigkeit += beschleunigung * Time.deltaTime;

                // Rotiere und bewege in Richtung Target
                if (zielErfasst)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, guidedRotation, drehung * Time.deltaTime);

                    rigidbody.velocity = transform.forward * geschwindigkeit;
            }

            // Zerstöre erst nach Ablauf der Lebenszeit
            if (TimeSince(abschussZeit) > lebenszeit)
                DestroyMissile(false);
        }
    }

    private void MissileGuidance()
    {
        Vector3 relativePosition = target.position - transform.position;
        float winkel = Mathf.Abs(Vector3.Angle(transform.forward.normalized, relativePosition.normalized)); // Liefert absoluten Betrag der reellen Position
        float distance = Vector3.Distance(target.position, transform.position); // Distanz zum Ziel ermitteln

        // Wenn das Ziel aus dem Sichtfeld (suchKegel) gerät, wurde der Rakete ausgewichen. Sofern die Distanz zu hoch ist, keine weitere Verfolgung.
        if (winkel > suchKegel || distance > suchReichweite)
            zielErfasst = false;
        // Sofern Zielerfassung aktiv ist, wird es verfolgt
        if (zielErfasst)
        {
            relativePosition = target.position - transform.position;
            guidedRotation = Quaternion.LookRotation(relativePosition, transform.up);
        }
    }

    // Aktiviere die Rakete
    private void ActivateMissile()
    {
        rigidbody.useGravity = false;  // Kann ab jetzt etwas treffen
        rigidbody.velocity = Vector3.zero;
        imFlug = true; // Solange Rakete aktiv ist, 'muss' sie auch fliegen

        // Falls keine Antriebszeit angegeben, ist Antrieb immer aktiv
        if (antriebsZeit <= 0.0f)  
            antrieb = true;

        aktivierungszeit = Time.time;
        geschwindigkeit = startGeschwindigkeit;
    }

    // Zerstöre die Rakete und starte Effekte, wenn etwas getroffn wurde
    private void DestroyMissile(bool impact)
    {
        Destroy(gameObject);

        if (missileSFX.SelfDestruct)
            missileSFX.Explode();
        else if (impact)
            missileSFX.Explode();
    }

    // Messung des zeitlichen Abstands
    private float TimeSince(float since)
    {
        return Time.time - since;
    }
}
