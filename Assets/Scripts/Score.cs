// Score und Highscore

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	[SerializeField] int score;
	[SerializeField] int hiScore;
    [SerializeField] Text hiScoreMenu;
    [SerializeField] Text hiScorePauseMenu;
    [SerializeField] Text hiScoreText;
	[SerializeField] Text scoreText;

    private void Awake()
    {
        hiScore = PlayerPrefs.GetInt("hiScore", 0);
        hiScoreMenu.text = hiScore.ToString() + "\n\n\n";
    }


    void OnEnable(){
		EventManager.onStartGame += ResetScore;
        EventManager.onStartGame += LoadHiScore;
        EventManager.onPlayerDeath += CheckNewHiScore;
		EventManager.onScorePoints += AddScore;
	}

	void OnDisable(){
		EventManager.onStartGame -= ResetScore;
        EventManager.onStartGame -= LoadHiScore;
        EventManager.onPlayerDeath -= CheckNewHiScore;
		EventManager.onScorePoints -= AddScore;
	}

	void ResetScore(){
		score = 0;
		DisplayScore ();
	}

	void AddScore(int amt){
		score += amt;
        DisplayScore();
    }

	void DisplayScore(){
		scoreText.text = score.ToString();
	}

	void LoadHiScore(){
		hiScore = PlayerPrefs.GetInt ("hiScore", 0);
        DisplayHightScore();
    }

	void CheckNewHiScore(){
		if (score > hiScore) {
			PlayerPrefs.SetInt ("hiScore", score);
			DisplayHightScore();
		}
	}

	void DisplayHightScore(){
		hiScoreText.text = "Highscore: " + hiScore.ToString();
        hiScorePauseMenu.text = hiScore.ToString() + "\n\n\n";
    }
}
