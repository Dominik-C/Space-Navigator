// Verwaltung der Menüs und den damit verbundenen Aktionen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    public static bool inSpiel = false;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject menuImage;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameHud;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject fuelSlider;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerPrefab2;
    [SerializeField] GameObject playerPrefab3;
    [SerializeField] GameObject playerPrefab4;
    [SerializeField] GameObject playerStartPosition;

    public Button shipSelectButton;
    public Button shipSelectButton2;
    public static bool inMenu = true;
    public static bool newStart = false; 
    public static bool playerDead = false;
    private bool spielerShiff2 = false;

    void OnEnable()
    {
        EventManager.onStartGame += ShowGameUI;
        EventManager.onPlayerDeath += ShowMainMenu;
    }

    void OnDisable()
    {
        EventManager.onStartGame -= ShowGameUI;
        EventManager.onPlayerDeath -= ShowMainMenu;
    }


    void Start(){
  
        Button btn1 = shipSelectButton.GetComponent<Button>();
        btn1.onClick.AddListener(SelectShip1);

        Button btn2 = shipSelectButton2.GetComponent<Button>();
        btn2.onClick.AddListener(SelectShip2);

        if (newStart == false)
        {
            MainMenu();           
            DelayMainMenuDisplay();
        }
        else
        {
            DelayMainMenuDisplay();
        }		
	}

    private void Awake()
    {
        Time.timeScale = 1;
        inSpiel = false;
    }

    void SelectShip1(){
        spielerShiff2 = false;
        inMenu = false;
        Instantiate(playerPrefab, playerStartPosition.transform.position, playerStartPosition.transform.rotation);
    }

    void SelectShip2(){
        spielerShiff2 = true;
        inMenu = false;
        Instantiate(playerPrefab2, playerStartPosition.transform.position, playerStartPosition.transform.rotation);
    }

    void SelectShip3()
    {
        //spielerShiff2 = true;
        inMenu = false;
        Instantiate(playerPrefab3, playerStartPosition.transform.position, playerStartPosition.transform.rotation);
    }

    void ShowMainMenu(){
        playerDead = true;
        inMenu = true;
        gameHud.SetActive(false);
        gameOverScreen.SetActive(true);
        Invoke ("DelayMainMenuDisplay", Asteroid.destructionDelay * 3); 
        // Warte 3 Sekunden bis die Animation zu Ende ist, zeige danach das MainMenu an
    }

    void MainMenu()
    {
        menuImage.SetActive(true);
    }

    void DelayMainMenuDisplay(){
        mainMenu.SetActive (true);
		gameUI.SetActive (false);
        gameHud.SetActive(false);
        fuelSlider.SetActive(false);
        gameOverScreen.SetActive(false);
        Cursor.visible = true;
    }

	void ShowGameUI(){
       // inMenu = false;
        menuImage.SetActive(false);
        mainMenu.SetActive (false);
		gameUI.SetActive (true);
        gameHud.SetActive(true);
        fuelSlider.SetActive(true);
        gameOverScreen.SetActive(false);
        Cursor.visible = false;
        playerDead = false;
    }

    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.Escape) && inSpiel == false && playerDead == false && newStart == true)
            {
                // Halte Zeit an, zeige Menü, verstecke Fadenkreuz sowie Spielcursor und zeige Mauscursor. 
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                gameHud.SetActive(false);
                Cursor.visible = true;
                inSpiel = true;
            }

            else if (Input.GetKeyDown(KeyCode.Escape) && playerDead == false && newStart == true)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                gameHud.SetActive(true);
                Cursor.visible = false;
                inSpiel = false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && playerDead == true)
            {
                return;
            }
        }
    }

 }
