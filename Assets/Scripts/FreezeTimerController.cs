using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeTimerController : MonoBehaviour {

    public Slider freezeTimer;
    private float currentPosition;
    private float modifier;
    private GameController gameController;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        modifier = 0.2f;
    }
	
	void FixedUpdate ()
    {
		if(currentPosition <= 1)
        {
            currentPosition += Time.deltaTime * modifier;
        }

        freezeTimer.value = currentPosition;

        if(freezeTimer.value >= 1)
        {
            gameController.gameOver = true;
        }
	}

    public void ResetTimer()
    {
        currentPosition = 0;
    }
}
