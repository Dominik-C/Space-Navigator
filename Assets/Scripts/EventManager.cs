// Event Manager

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	public delegate void StartGameDelegate();
	public static StartGameDelegate onStartGame;
	public static StartGameDelegate onPlayerDeath;
    public static StartGameDelegate onRespawnPickup;

    public delegate void TakeDamageDelegate(float amt);
	public static TakeDamageDelegate onTakeDamage;

	public delegate void ScorePointsDelegate(int amt);
	public static ScorePointsDelegate onScorePoints;

    public static void StartGame(){
		//Debug.Log ("Start mit dem SPiel");
		if (onStartGame != null) {
			onStartGame ();
		}
	}

    public static void ReSpawnPickup()
    {
        //ToDo -> Es sollten immer alle drei Pickups der Scene eingesammt sein, bevor es einen Respawn gibt. (Kann auch anders gelöst werden). 
        if (onRespawnPickup != null)
            onRespawnPickup();     
    }

    public static void TakeDamage(float percent){
		// Debug.Log ("Take DMG: " + percent);
		if (onTakeDamage != null) {
			onTakeDamage (percent);
		}
	}

    public static void PlayerDeath(){
        
		Debug.Log ("Player tot");
		if (onPlayerDeath != null) {
            onPlayerDeath ();
            // UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
	}

    public static void ScorePoints(int score){
		
		if (onScorePoints != null) {
			onScorePoints (score);
		}
	}
}
