using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
	[SerializeField] float timePassed;
	bool keepTime = false;
	[SerializeField] Text timerText;

    // Eventlistener
	void OnEnable()
    {
		EventManager.onStartGame += StartTimer;
		EventManager.onPlayerDeath += StopTiemer;
	}

	void OnDisable()
    {
		EventManager.onStartGame -= StartTimer;
		EventManager.onPlayerDeath -= StopTiemer;
	}

	void Update()
    {
		if (keepTime)
        {
			timePassed += Time.deltaTime;
			UpdateTimerDisplay ();
		}
	}

	void StartTimer()
    {
		timePassed = 0;
		keepTime = true;
	}

	void StopTiemer()
    {
		keepTime = false;
	}

	void UpdateTimerDisplay()
    {
		// Kovertieren in Minuten und Sekunden
		int minutes;
		float secound;

		minutes = Mathf.FloorToInt(timePassed/60);
		secound = timePassed % 60;

        // Ausgabe auf Bildschirm
		timerText.text = string.Format("{0}:{1:00.00}", minutes, secound);
	}
}
