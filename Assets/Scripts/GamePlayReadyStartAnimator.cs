using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayReadyStartAnimator : MonoBehaviour {
    public float showReadyDuration = 1.5f;
    public float showStartDuration = 1.5f;
    public bool finished = false;
    Text readyStartText;

    private bool animating = false;
    private bool showingReady = false;
    private bool showingStart = false;
    private float timeSinceAnimationStarted = 0;


    void Start()
    {
        readyStartText = GameObject.FindWithTag("GamePlayReadyStartText").GetComponent<Text>();
    }

    public void StartAnimation()
    {
        animate();
    }

    void FixedUpdate()
    {
        if (animating)
        {
            timeSinceAnimationStarted += Time.deltaTime;
            animate();
        }
    }

    void animate()
    {
        // this is to avoid the first frame of FixedUpdate running
        // before readyStartText is instantiated
        if (animating)
        {

            if (!showingReady)
            {
                // show Ready
                readyStartText.text = "Ready";
                showingReady = true;
            }

            if (timeSinceAnimationStarted >= showReadyDuration && !showingStart)
            {
                readyStartText.text = "Start";
                // hide Ready
                // show Start
                showingStart = true;
            }

            if (timeSinceAnimationStarted >= showReadyDuration + showStartDuration)
            {
                // hide Start

                finished = true;
                readyStartText.text = "";
                gameObject.GetComponent<GamePlayReadyStartAnimator>().enabled = false;
            }
        }


        animating = true;
    }
}
