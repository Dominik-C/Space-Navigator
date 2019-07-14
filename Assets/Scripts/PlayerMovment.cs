// Verwaltung von Eingaben zur Spielerschiffbewegung, sowie Maussteuerung

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerMovment : MonoBehaviour {

    public bool useMouseInput = true; // ToDo -> Ein und ausschaltbar via Menuoption.
    [SerializeField] float movementSpeed = 40f;
	[SerializeField] float turnSpeed = 60f;
	[SerializeField]Thruster[] thruster;
    Transform myT;
 
    // Gültigkeitsbereiche der Maussteuerung
    [Range(-1, 1)]
    public float mousePitch;
    [Range(-1, 1)]
    public float mouseYaw;
    [Range(-1, 1)]
    public float rollen;
    [Range(-1, 1)]
    public float seitwaerts;

    private ShipMovment ship;
    FuelManager fuelSystem;


    void Awake()
    {
        ship = GetComponent<ShipMovment>();
        myT = transform;
    }

    void Start()
    {
        fuelSystem = GetComponent<FuelManager>();
    }

    void Update () {
        // Stoppe bewegung wenn Tank leer ist
        if (fuelSystem.startFuel <= 0)
        {
            EventManager.PlayerDeath();
            GetComponent<Explosion>().BlowUp(); // Only temporarly
        }

        // Verbraucht Tank bei Tastendruck W
        if (Input.GetKey(KeyCode.W) && GameUI.inSpiel == false)
        {
            fuelSystem.tankVerbrauch = movementSpeed * 0.2f;
            fuelSystem.ReduceFuel();
        }

        // Rückwärts fliegen
        if (Input.GetKey(KeyCode.S) && GameUI.inSpiel == false)
        {
            transform.position -= transform.forward / 6;
            fuelSystem.tankVerbrauch = movementSpeed * .05f;
            fuelSystem.ReduceFuel();
        }

        // Schiff heben
        if (Input.GetKey(KeyCode.T) && GameUI.inSpiel == false)
        {
            transform.Translate(Vector3.up * 0.15f);
            fuelSystem.tankVerbrauch = movementSpeed * .05f;
            fuelSystem.ReduceFuel();
        }

        // Schiff absenken
        if (Input.GetKey(KeyCode.G) && GameUI.inSpiel == false)
        {
            transform.Translate(Vector3.down * 0.15f);
            fuelSystem.tankVerbrauch = movementSpeed * .05f;
            fuelSystem.ReduceFuel();
        }

        if (useMouseInput)
        {
            seitwaerts = Input.GetAxis("Horizontal");
            mouseMovment();
            Thrust ();
		    Turn ();
        }
        else
        {
            Thrust();
            Turn();
        }
    }

    private void mouseMovment()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePitch = (mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f);
        mouseYaw = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f);
        mousePitch = -Mathf.Clamp(mousePitch, -1.0f, 1.0f);
        mouseYaw = Mathf.Clamp(mouseYaw, -1.0f, 1.0f);
    }

    void Turn()
    {
		float yaw = turnSpeed * Time.deltaTime * Input.GetAxis("Horizontal"); 
		float pitch = turnSpeed * Time.deltaTime * Input.GetAxis("Pitch");
		float roll = turnSpeed * Time.deltaTime * Input.GetAxis("Roll"); 
		myT.Rotate (-pitch,yaw,roll);
	}

    void Thrust()
    {
		if (Input.GetAxis ("Vertical") > 0) 
			myT.position += myT.forward * movementSpeed * Time.deltaTime * Input.GetAxis ("Vertical");

			foreach (Thruster t in thruster)
				t.Intensity (Input.GetAxis("Vertical"));
    }
}
