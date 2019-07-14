// Zuständig für die Verwaltung der Spieler Kameras. (3rd & 1st Person)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public GameObject camera3rd;     // 3rd Person
    public GameObject camera1st;     // 1st Person
    public static bool cameraFirst = false;
    AudioListener camera3rdAudioListener;
    AudioListener camera1stAudioListener;

    // Use this for initialization
    void Start()
    {
        //Kamera Listeners holen
        camera3rdAudioListener = camera3rd.GetComponent<AudioListener>();
        camera1stAudioListener = camera1st.GetComponent<AudioListener>();
        //Kamera Position holen
        cameraPositionChange(PlayerPrefs.GetInt("CameraPosition"));
    }

    // Update is called once per frame
    void Update()
    {
        switchCamera();
    }

    public void cameraPositonM()
    {
        cameraChangeCounter();
    }

    //Change Camera Keyboard
    void switchCamera()
    {
        if (Input.GetKeyDown(KeyCode.C) && GameUI.inSpiel == false)
            cameraChangeCounter();
    }

    //Camera Counter
    void cameraChangeCounter()
    {
        int cameraPositionCounter = PlayerPrefs.GetInt("CameraPosition");
        cameraPositionCounter++;
        cameraPositionChange(cameraPositionCounter);
    }

    // Zwischen beiden Kameras wechseln
    void cameraPositionChange(int camPosition)
    {
        if (camPosition > 1)
            camPosition = 0;

        //Kamera Position setzen
        PlayerPrefs.SetInt("CameraPosition", camPosition);

        // 'Enable' alle Teile einzeln, anstelle von rekursiv, da Parent Objekt keinen MeshRenderer hat
        if (camPosition == 0) // 3rd Perosn 
        {
            GameObject.Find("PlayerShip1").GetComponent<Renderer>().enabled = true;
            GameObject.Find("CockpitBase").GetComponent<Renderer>().enabled = false;
            GameObject.Find("CockpitGlass").GetComponent<Renderer>().enabled = false;
            GameObject.Find("CockpitHUD").GetComponent<Renderer>().enabled = false;
            GameObject.Find("CockpitJoystick").GetComponent<Renderer>().enabled = false;
            GameObject.Find("CockpitPanel").GetComponent<Renderer>().enabled = false;
            GameObject.Find("CockpitPanel_R").GetComponent<Renderer>().enabled = false;
            GameObject.Find("CockpitPanel3").GetComponent<Renderer>().enabled = false;
          
            camera3rd.SetActive(true);
            camera3rdAudioListener.enabled = true;
            camera1stAudioListener.enabled = false;           
            camera1st.SetActive(false);
            cameraFirst = false;
        }

        if (camPosition == 1) // 1st Person
        {
            GameObject.Find("CockpitBase").GetComponent<Renderer>().enabled = true;
            GameObject.Find("CockpitGlass").GetComponent<Renderer>().enabled = true;
            GameObject.Find("CockpitHUD").GetComponent<Renderer>().enabled = true;
            GameObject.Find("CockpitJoystick").GetComponent<Renderer>().enabled = true;
            GameObject.Find("CockpitPanel").GetComponent<Renderer>().enabled = true;
            GameObject.Find("CockpitPanel_R").GetComponent<Renderer>().enabled = true;
            GameObject.Find("CockpitPanel3").GetComponent<Renderer>().enabled = true;
            GameObject.Find("PlayerShip1").GetComponent<Renderer>().enabled = false;


            camera1st.SetActive(true);
            camera1stAudioListener.enabled = true;
            camera3rdAudioListener.enabled = false;
            cameraFirst = true;
            camera3rd.SetActive(false);
        }

    }
}