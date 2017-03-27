using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    FreezeTimerController freezeTimerController;

    public Text swipesNeeded;
    public Text playerSwipes;

    public enum Directions
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }



    int[] swipesToClearThisTile = new int[100];
    int[] playerSwipesForThisTile = new int[100];
    int numberOfPlayerSwipesForTheCurrentTile = 0;
    int difficultyLevel = 1;
    private bool tileFinished = false;
    public bool gameOver = false;

    private int numberOfSwipesNeededToCompleteCurrentTile;

    int tilePlayerIsOn = 0;

    int numberOfTilesTilDifficultyIncrease = 4;

    public float maxTime = 0.5f;
    public float minSwipeDist = 50f;

    float startTime;
    float endTime;

    Vector3 startPos;
    Vector3 endPos;

    float swipeDistance;
    float swipeTime;

    void Start()
    {
        // for testing purposes:
        // we will always act like this is the first time playing the game
        PlayerPrefs.DeleteKey("hasPlayedFirstGame");

        // first time ever playing this game
        // we need to set the player prefs to the initial values
        checkIfFirstTimePlayingInit();

        freezeTimerController = GameObject.FindWithTag("GameController").GetComponent<FreezeTimerController>();
        buildSwipes();
        playerSwipes.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (tileFinished)
        {
            buildSwipes();
            tileFinished = false;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTime = Time.time;
                startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTime = Time.time;
                endPos = touch.position;

                swipeDistance = (endPos - startPos).magnitude;
                swipeTime = endTime - startTime;

                if (swipeTime < maxTime && swipeDistance > minSwipeDist)
                {
                    swipe();

                    if (numberOfPlayerSwipesForTheCurrentTile == numberOfSwipesNeededToCompleteCurrentTile)
                    {
                        numberOfPlayerSwipesForTheCurrentTile = 0;
                        playerSwipes.text = "";
                        buildSwipes();
                        freezeTimerController.ResetTimer();
                    }
                }
            }
        }


        if (gameOver)
        {
            //Debug.Log("GameOver");
        }
    }

    void swipe()
    {
        // need to check if it is a vertical swipe or horizontal swipe first
        Vector2 distance = endPos - startPos;

        int swipeDirection = 0;

        if (Mathf.Abs(distance.x) > Mathf.Abs(distance.y))
        {
            if (distance.x > 0)
            {
                swipeDirection = (int) Directions.Right;
                Debug.Log("Right Swipe");
            }
            else if (distance.x < 0)
            {
                swipeDirection = (int)Directions.Left;
            }
        }
        else if (Mathf.Abs(distance.x) < Mathf.Abs(distance.y))
        {
            if (distance.y > 0)
            {
                swipeDirection = (int)Directions.Up;
            }
            else if (distance.y < 0)
            {
                swipeDirection = (int)Directions.Down;
            }
        }

        // if the player swipes the wrong direction
        if(swipesToClearThisTile[numberOfPlayerSwipesForTheCurrentTile] != swipeDirection)
        {
            // start their swiping over for this tile
            playerSwipes.text = "";
            numberOfPlayerSwipesForTheCurrentTile = 0;
        }
        else
        {
            
            playerSwipesForThisTile[numberOfPlayerSwipesForTheCurrentTile] = swipeDirection;

            if (swipeDirection == (int)Directions.Up)
            {
                playerSwipes.text += "Up ";
            }
            else if (swipeDirection == (int)Directions.Down)
            {
                playerSwipes.text += "Down ";
            }
            else if (swipeDirection == (int)Directions.Left)
            {
                playerSwipes.text += "Left ";
            }
            else if (swipeDirection == (int)Directions.Right)
            {
                playerSwipes.text += "Right ";
            }
            numberOfPlayerSwipesForTheCurrentTile++;
        }
        
    }

    // Use this for initialization
    

    private void buildSwipes()
    {
        swipesNeeded.text = "";
        int shovelLevel = PlayerPrefs.GetInt("shovelLevel");
        numberOfSwipesNeededToCompleteCurrentTile = difficultyLevel - shovelLevel + (tilePlayerIsOn / numberOfTilesTilDifficultyIncrease);

        for(int i=0; i<numberOfSwipesNeededToCompleteCurrentTile; i++)
        {
            int swipe = (int)Mathf.Round(Random.Range(-0.51f, 3.49f));

            if (swipe == (int) Directions.Up)
            {
                swipesToClearThisTile[i] = (int) Directions.Up;
                swipesNeeded.text += "Up ";
            }
            else if (swipe == (int)Directions.Down)
            {
                swipesToClearThisTile[i] = (int)Directions.Down;
                swipesNeeded.text += "Down ";
            }
            else if (swipe == (int)Directions.Left)
            {
                swipesToClearThisTile[i] = (int)Directions.Left;
                swipesNeeded.text += "Left ";
            }
            else if (swipe == (int)Directions.Right)
            {
                swipesToClearThisTile[i] = (int)Directions.Right;
                swipesNeeded.text += "Right ";
            }
        }
    }

    private void checkIfFirstTimePlayingInit()
    {
        string hasPlayedFirstGame = PlayerPrefs.GetString("hasPlayedFirstGame");

        if (hasPlayedFirstGame != "True")
        {
            PlayerPrefs.SetString("hasPlayedFirstGame", "True");
            PlayerPrefs.SetInt("difficultyLevel", 6);
            PlayerPrefs.SetInt("shovelLevel", 2);
            
            PlayerPrefs.Save();

            difficultyLevel = PlayerPrefs.GetInt("difficultyLevel");
        }
    }
}
