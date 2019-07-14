// Tanksystem für das Raumschiff - Bei leerem Tank -> GameOver

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour {

    public float startFuel;         // Tankfüllung Spielanfang
    public float maxFuel = 1000f;   // Maximale Tankfüllung
    public float tankVerbrauch;     // Bestimmt wieviel Energie verbraucht wird
    private Slider tankSlider;
    private Text tankTxt;

    void Start ()
    {
        // Tank Obergrenze
        if (startFuel > maxFuel)
            startFuel = maxFuel;

        tankSlider.maxValue = maxFuel;
        UpdateUI();
    }

    void Awake()
    {
        tankTxt = GameObject.FindWithTag("FuelText").GetComponent<Text>();
        tankSlider = GameObject.FindWithTag("FuelSlider").GetComponent<Slider>();
    }

    // Tankfüllung aufbrauchen und UI updaten
    public void ReduceFuel()
    {
        startFuel -= Time.deltaTime * tankVerbrauch;
        UpdateUI();
    }

    // Ausgabe auf dem Bildschirm
    void UpdateUI ()
    {
        tankSlider.value = startFuel;
        tankTxt.text = "Fuel: " + startFuel.ToString("0") + "\nShield" ;

        if (startFuel <=0)
        {
            startFuel = 0;
            tankTxt.text = "Out of fuel";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup") || other.CompareTag("Handplaced"))
        {
            startFuel += 200f;
            if(startFuel >= maxFuel)
                startFuel = maxFuel;
            UpdateUI();
        }
    }
}
