// Wird nicht mehr benötigt, da alles in einer Scene ist. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour {

    private GameObject player;
    public static bool restarted = false;
    public Explosion blow;

    private void Start()
    {
        blow = GameObject.FindObjectOfType(typeof(Explosion)) as Explosion;
        restarted = false;
    }

    public void Click()
    {            
        restarted = true;
        Time.timeScale = 1;
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        // UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void SeRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
