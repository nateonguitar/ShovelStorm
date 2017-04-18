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
    bool paused = false;
    float maxCoatModifierLevel = 20f;
    float timeThisBarTook = 0f;

    void Start()
    {
        gamePlayReadyStartAnimator = GameObject.FindWithTag("GamePlayController").GetComponent<GamePlayReadyStartAnimator>();
        gameController = GameObject.FindWithTag("GamePlayController").GetComponent<GamePlayController>();
        modifier = 1f - (float)PlayerPrefs.GetInt("coatLevel") / maxCoatModifierLevel;
        modifier *= 1f + (((float)PlayerPrefs.GetInt("neighborhoodChosenFromMap")* 1.1f)/ maxCoatModifierLevel);
        modifier *= 0.5f;
        freezeTimer.value = 0;
    }
	
	void FixedUpdate ()
    {
        timeThisBarTook += Time.deltaTime;
        if (gamePlayReadyStartAnimator.finished && !paused)
        {
            if (currentPosition <= 1)
            {
                currentPosition += Time.deltaTime * modifier;
            }

            freezeTimer.value = currentPosition;

            if (freezeTimer.value >= 1)
            {
                gameController.gameOver = true;
                Debug.Log(timeThisBarTook);
            }
        }
	}

    public void ResetTimer()
    {
        currentPosition = 0;
        timeThisBarTook = 0;
    }

    public void PauseTimer()
    {
        paused = true;
    }

    public void UnPauseTimer()
    {
        paused = false;
    }
}
