using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeTimerController : MonoBehaviour {

    public Slider freezeTimer;
    private float currentPosition;
    private float modifier;
    private GamePlayController gameController;
    GamePlayReadyStartAnimator gamePlayReadyStartAnimator;

    void Start()
    {
        gamePlayReadyStartAnimator = GameObject.FindWithTag("GamePlayController").GetComponent<GamePlayReadyStartAnimator>();
        gameController = GameObject.FindWithTag("GamePlayController").GetComponent<GamePlayController>();
        modifier = 0.2f;
        freezeTimer.value = 0;
    }
	
	void FixedUpdate ()
    {
        if (gamePlayReadyStartAnimator.finished)
        {
            if (currentPosition <= 1)
            {
                currentPosition += Time.deltaTime * modifier;
            }

            freezeTimer.value = currentPosition;

            if (freezeTimer.value >= 1)
            {
                gameController.gameOver = true;
            }
        }
	}

    public void ResetTimer()
    {
        currentPosition = 0;
    }
}
